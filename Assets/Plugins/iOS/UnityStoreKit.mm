/*
 *  The MIT License (MIT)
 *
 *	Copyright (c) 2014 Mateusz Majchrzak
 *
 *	Permission is hereby granted, free of charge, to any person obtaining a copy
 *	of this software and associated documentation files (the "Software"), to deal
 *	in the Software without restriction, including without limitation the rights
 *	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 *	copies of the Software, and to permit persons to whom the Software is
 *	furnished to do so, subject to the following conditions:
 *
 *	The above copyright notice and this permission notice shall be included in all
 *	copies or substantial portions of the Software.
 *
 *	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 *	SOFTWARE.
 */

#import "UnityStoreKit.h"

#define HANDLE_REQUEST_SUCCESS      @"HandleRequestSuccess"
#define HANDLE_REQUEST_FAILED       @"HandleRequestFailed"
#define HANDLE_TRANSACTION_SUCCESS  @"HandleTransactionSuccess"
#define HANDLE_TRANSACTION_FAILED   @"HandleTransactionFailed"
#define HANDLE_RESTORE              @"HandleTransactionRestore"

#define STR_TO_NSTRING(str) [NSString stringWithUTF8String: str ? str : ""]

char *c_strcpy(const char *string) {
    if (string == NULL)
        return NULL;
    
    char *res = (char *)malloc(strlen(string) + 1);
    strcpy(res, string);
    
    return res;
}

#ifdef __cplusplus
extern "C" {
#endif
	void UnitySendMessage(const char *className, const char *methodName, const char *param);
#ifdef __cplusplus
}
#endif

@implementation UnityStoreKit

/// UnityStoreKit singleton instance.
+ (instancetype) sharedUnityStoreKit
{
    static id instance = nil;
    
    @synchronized(self) {
        if (instance == nil)
            instance = [[self alloc] init];
    }
    
    return instance;
}

/// Sends message to Unity gameObject.
- (void)sendMessage:(NSString *)methodName withData:(NSString *)data
{
    if (!_targetClass)
        return;
    
    UnitySendMessage([_targetClass UTF8String], [methodName UTF8String], data != nil ? [data UTF8String] : "");
}

/// Sends message to Unity gameObject.
- (void)sendMessage:(NSString *)methodName
{
    [self sendMessage:methodName withData:nil];
}

/// Checks that device is able to make payments.
- (BOOL)canMakePayments
{
    return [SKPaymentQueue canMakePayments];
}

/// Request products data from AppStore.
- (void)request:(NSSet *)products
{
    if (![self canMakePayments]) {
        [self sendMessage:HANDLE_REQUEST_FAILED withData:@"Payments are not available!"];
        return;
    }
    
    if (_currentRequest) {
        ///
        return;
    }
    
    _products = nil;
    
    _currentRequest = [[SKProductsRequest alloc] initWithProductIdentifiers:products];
    _currentRequest.delegate = self;
    
    [_currentRequest start];
}

// StoreKit request handle.
- (void)productsRequest:(SKProductsRequest *)request didReceiveResponse:(SKProductsResponse *)response
{
    _products = [response.products copy];
    
    [self sendMessage:HANDLE_REQUEST_SUCCESS];
}

// StoreKit request handle.
- (void)request:(SKRequest *)request didFailWithError:(NSError *)error
{
    _products = nil;
    
    [self sendMessage:HANDLE_REQUEST_FAILED withData:error.localizedDescription];
}

// StoreKit request handle.
- (void)requestDidFinish:(SKRequest *)request
{
    _currentRequest = nil;
}

// Purchases specific product
- (void)purchase:(NSString *)productIdentfifer
{
    if (![self canMakePayments]) {
        [self sendMessage:HANDLE_TRANSACTION_FAILED withData:productIdentfifer];
        
        return;
    }
    
    SKPayment *payment = [SKPayment paymentWithProductIdentifier:productIdentfifer];
    
    [[SKPaymentQueue defaultQueue] addPayment:payment];
}

// StoreKit purchase handle.
-(void)paymentQueue:(SKPaymentQueue *)queue updatedTransactions:(NSArray *)transactions
{
    for (SKPaymentTransaction *transaction in transactions)
    {
        switch (transaction.transactionState)
        {
            case SKPaymentTransactionStatePurchasing:
                break;
            case SKPaymentTransactionStatePurchased:
                [self sendMessage:HANDLE_TRANSACTION_SUCCESS withData:transaction.payment.productIdentifier];
                
                [[SKPaymentQueue defaultQueue] finishTransaction:transaction];
                break;
            case SKPaymentTransactionStateFailed:
                [self sendMessage:HANDLE_TRANSACTION_FAILED withData:transaction.payment.productIdentifier];
                
                [[SKPaymentQueue defaultQueue] finishTransaction:transaction];
                break;
            case SKPaymentTransactionStateRestored:
                [self sendMessage:HANDLE_RESTORE withData:transaction.originalTransaction.payment.productIdentifier];
                
                [[SKPaymentQueue defaultQueue] finishTransaction:transaction];
                break;
        }
    }
}

// Restore purchases.
- (void)restore
{
    [[SKPaymentQueue defaultQueue] restoreCompletedTransactions];
}

@end

extern "C"
{
    struct USKProduct {
        char *identifier;
        char *title;
        char *price;
        char *description;
    };
    
    // Initializes StoreKit
    void USKInit(const char *targetClass) {
        UnityStoreKit *usk =[UnityStoreKit sharedUnityStoreKit];
        
        usk.targetClass = [NSString stringWithUTF8String:targetClass];
        
        [[SKPaymentQueue defaultQueue] addTransactionObserver:usk];
    }
    
    // Checks that you can make pyments.
    bool USKCanMakePayments() {
        return [[UnityStoreKit sharedUnityStoreKit] canMakePayments];
    }
    
    // Sends an request for products informations to AppStore.
    void USKRequest(char *identifiers[], int length) {
        NSMutableSet *ids = [[NSMutableSet alloc] init];
        
        for (int i = 0; i < length; i++) {
            [ids addObject:STR_TO_NSTRING(identifiers[i])];
        }
        
        [[UnityStoreKit sharedUnityStoreKit] request:ids];
    }
    
    // Returns cashed products list.
    int USKGetProducts(USKProduct **products) {
        NSArray *response = [UnityStoreKit sharedUnityStoreKit].products;
        
        if (response == nil || response.count == 0) {
            *products = NULL;
            
            NSLog(@"UnityStoreKit: Fetched products list is empty!");
            
            return 0;
        }
        
        *products = (USKProduct *)malloc(sizeof(USKProduct) * response.count);
        
        int i = 0;
        for (SKProduct *product in [UnityStoreKit sharedUnityStoreKit].products) {
            USKProduct &current = *products[i];
            
            current.identifier = c_strcpy([product.productIdentifier UTF8String]);
            current.title = c_strcpy([product.localizedTitle UTF8String]);
            current.description = c_strcpy([product.localizedDescription UTF8String]);
            
            // Create localized price string.
            NSNumberFormatter *formatter = [[NSNumberFormatter alloc] init];
            [formatter setFormatterBehavior:NSNumberFormatterBehavior10_4];
            [formatter setNumberStyle:NSNumberFormatterCurrencyStyle];
            [formatter setLocale:product.priceLocale];
            
            current.price = c_strcpy([[formatter stringFromNumber:product.price] UTF8String]);
            
            i++;
        }
        
        return response.count;
    }
    
    //
    void USKPurchase(const char* productIdentifier) {
        [[UnityStoreKit sharedUnityStoreKit] purchase:STR_TO_NSTRING(productIdentifier)];
    }
    
    //
    void USKRestore() {
        [[UnityStoreKit sharedUnityStoreKit] restore];
    }
}