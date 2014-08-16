StoreKit plugin for Unity 3D
=========
---
Simple and complex Apple StoreKit implementation:

  - Buy consumable and non-consumable products.
  - Fetch products from AppStore.
  - Restore finished transactions.

Bootstrap
--------------

Create IStoreDelegate implementation:
```sh
public class StoreService : Store.IStoreDelegate
{
    /// Handles the request success event.
    void OnStoreRequestSuccess(IEnumerable<Store.StoreProduct> products)
    {
    }
    		
    /// Handles the request failed event.
    void OnStoreRequestFailed(string error)
    {
    }
    		
    /// Handles the transaction success event.
    void OnStoreTransactionSuccess(string identifier)
    {
    }
    		
    /// Handles the transaction failed event.
    void OnStoreTransactionFailed(string identifier)
    {
    }
    
    /// Handles the transaction restore event.
    void OnStoreTransactionRestore(string identifier)
    {
    }
}
```
Instantiate StoreKit and set IStoreDelegate target:
```sh
private void Awake()
{
    Store.StoreKit.Instance.Delegate = new StoreService();
}
```

Request and buy product:
```sh
private string m_ProductID = '"com.game.product";

private void Awake()
{
    Store.IStore store = Store.StoreKit.Instance;

    if (!store.IsAvailable)
        return;

    store.Request(new [] { m_ProductID });
    store.Purchase(m_ProductID);
}
```

Version
----

0.9

License
----

MIT
