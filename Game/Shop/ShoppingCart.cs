﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingCart : CustomMonoBehaviour {
    private BuySellAnimal buySellAnimal;
    private CanvasController canvasController;
    private DialogueManager dialogueManager;
    private Inventory shoppingCart;
    public Inventory source;
    public List<Item> items;
    public Text confirm;
    public bool buy;
    public List<ItemDetails> itemDetailsList;
    public GameObject shop;
    private int id;

    void Start() {
        shop = transform.parent.parent.gameObject;
        shoppingCart = ScriptableObject.CreateInstance<Inventory>();
        canvasController = centralController.Get("CanvasController").GetComponent<CanvasController>();
        dialogueManager = centralController.Get("DialogueManager").GetComponent<DialogueManager>();
        buySellAnimal = centralController.Get("AnimalBuySell").GetComponent<BuySellAnimal>();
        foreach (ItemDetails item in itemDetailsList) {
            item.shop = shop;
            if (shop.TryGetComponent(out VetInformation vetInfo)) {
                item.vet = shop;
            }
        }
        ResetButton();
        RenderItems();
    }
    public void AddItemToShoppingCart(Item item) {
        if (shoppingCart.items.ContainsKey(item) && source.items[item] <= shoppingCart.items[item]) {
            canvasController.initiateNotification("Aw sorry, it looks like that's all of that item.", true);
        } else {
            shoppingCart.Additem(item);
            items = new List<Item>(shoppingCart.items.Keys);
            confirm.text = buy ? "Buy for: $" + shoppingCart.TotalCost(buy: true) : "Sell for: $" + shoppingCart.TotalCost(buy: false);
            RenderItems();
        }
    }
    public void RemoveItemFromShoppingCart(Item item) {
        Debug.Log("Remove :" + item.itemName);
        shoppingCart.RemoveAllItem(item);
        items = new List<Item>(shoppingCart.items.Keys);
        confirm.text = buy ? "Buy for: $" + shoppingCart.TotalCost(buy: true) : "Sell for: $" + shoppingCart.TotalCost(buy: false);
        RenderItems();
    }
    public void ResetButton() {
        confirm.text = "Empty Cart";
    }
    public void MakePurchase() {
        if (buySellAnimal.Checkout(shoppingCart)) {
            shoppingCart.Clear();
            items = new List<Item>(shoppingCart.items.Keys);
            if (shop.TryGetComponent(out ShopInformation shopInfo)) {
                shopInfo.updateList();
            } else if (shop.TryGetComponent(out VetInformation vetInfo)) {
                vetInfo.updateList();
            }
            ResetButton();
            RenderItems();
        } else {
            canvasController.initiateNotification("Aw sorry, it looks like you don't have enough on you..", true);
        };
    }
    public void MakeSale() {
        buySellAnimal.SellItems(shoppingCart);
        shoppingCart.Clear();
        items = new List<Item>(shoppingCart.items.Keys);
        if (shop.TryGetComponent(out ShopInformation shopInfo)) {
            shopInfo.updateList();
        } else if (shop.TryGetComponent(out VetInformation vetInfo)) {
            vetInfo.updateList();
        }
        ResetButton();
        RenderItems();
    }
    private void RenderItems() {
        for (int i = 0; i < System.Math.Min(items.Count, itemDetailsList.Count); ++i) {
            int idx = (id + i) % items.Count;
            itemDetailsList[i].gameObject.SetActive(true);
            itemDetailsList[i].item = items[idx];
            itemDetailsList[i].itemImage.sprite = items[idx].ItemSprite;
            itemDetailsList[i].quantity.text = shoppingCart.items[items[idx]].ToString();
        }
        if (itemDetailsList.Count > items.Count) {
            for (int i = 1; i <= itemDetailsList.Count - items.Count; i++) {
                itemDetailsList[itemDetailsList.Count - i].gameObject.SetActive(false);
            }
        }
    }
    public void ItemNext() {
        id++;
        if (id >= shoppingCart.items.Count) {
            id = 0;
        }
        RenderItems();
    }

    public void ItemPrevious() {
        id--;
        if (id < 0) {
            id = shoppingCart.items.Count - 1;
        }
        RenderItems();
    }
}
