StoreKit plugin for Unity 3D
=========
---
Simple and complex Apple StoreKit implementation:

  - Buy consumable and non-consumable products.
  - Fetch products from AppStore.
  - Restore finished transactions.

Installation
--------------

```sh
/// <summary>
/// Handles the request success event.
/// </summary>
/// <param name="identifier">product identifier.</param>
void OnStoreRequestSuccess(IEnumerable<StoreProduct> products);
		
/// <summary>
/// Handles the request failed event.
/// </summary>
void OnStoreRequestFailed(string error);
		
/// <summary>
/// Handles the transaction success event.
/// </summary>
/// <param name="identifier">product identifier.</param>
void OnStoreTransactionSuccess(string identifier);
		
/// <summary>
/// Handles the transaction failed event.
/// </summary>
/// <param name="identifier">product identifier.</param>
void OnStoreTransactionFailed(string identifier);
		
/// <summary>
/// Handles the transaction restore event.
/// </summary>
/// <param name="identifier">product identifier.</param>
void OnStoreTransactionRestore(string identifier);
```

Version
----

0.9

License
----

MIT
