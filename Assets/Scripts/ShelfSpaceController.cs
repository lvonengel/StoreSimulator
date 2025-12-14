using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Handles each space where items can be placed onto a shelf.
/// </summary>
public class ShelfSpaceController : MonoBehaviour {
    public StockInfo info;

    public List<StockObject> objectsOnShelf;

    public List<Transform> bigDrinkPoints;
    public List<Transform> cerealPoints, tubeChipsPoints, fruitPoints, largeFruitPoints;

    public TMP_Text shelfLabel;

    public void PlaceStock(StockObject objectToPlace) {
        bool preventPlacing = true;

        if (objectsOnShelf.Count == 0) {
            info = objectToPlace.info;
            preventPlacing = false;
            
        } else {
            if (info.name == objectToPlace.info.name) {
                preventPlacing = false;

                switch(info.typeOfStock) {
                    case StockInfo.StockType.bigDrink:
                        if (objectsOnShelf.Count >= bigDrinkPoints.Count) {
                            preventPlacing = true;
                        }

                        break;

                    case StockInfo.StockType.cereal:
                        if (objectsOnShelf.Count >= cerealPoints.Count) {
                            preventPlacing = true;
                        }

                        break;

                    case StockInfo.StockType.chipsTube:
                        if (objectsOnShelf.Count >= tubeChipsPoints.Count) {
                            preventPlacing = true;
                        }
                        break;

                    case StockInfo.StockType.fruit:
                        if (objectsOnShelf.Count >= fruitPoints.Count) {
                            preventPlacing = true;
                        }
                        break;

                    case StockInfo.StockType.fruitLarge:
                        if (objectsOnShelf.Count >= largeFruitPoints.Count) {
                            preventPlacing = true;
                        }
                        break;
                }

                
            }
        }


        if (preventPlacing == false) {
            objectToPlace.MakePlaced();

            switch (info.typeOfStock) {
                case StockInfo.StockType.bigDrink:
                    objectToPlace.transform.SetParent(bigDrinkPoints[objectsOnShelf.Count]);
                    break;

                case StockInfo.StockType.cereal:
                    objectToPlace.transform.SetParent(cerealPoints[objectsOnShelf.Count]);
                    break;

                case StockInfo.StockType.chipsTube:
                    objectToPlace.transform.SetParent(tubeChipsPoints[objectsOnShelf.Count]);
                    break;

                case StockInfo.StockType.fruit:
                    objectToPlace.transform.SetParent(fruitPoints[objectsOnShelf.Count]);
                    break;

                case StockInfo.StockType.fruitLarge:
                    objectToPlace.transform.SetParent(largeFruitPoints[objectsOnShelf.Count]);
                    break;
            }

            objectsOnShelf.Add(objectToPlace);
            UpdateDisplayPrice(info.currentPrice);
        }
    }

    public StockObject GetStock() {
        StockObject objectToReturn = null;

        if (objectsOnShelf.Count > 0) {
            objectToReturn = objectsOnShelf[objectsOnShelf.Count - 1];
            objectsOnShelf.RemoveAt(objectsOnShelf.Count - 1);
        }

        if (objectsOnShelf.Count == 0) {
            shelfLabel.text = string.Empty;
        }

        return objectToReturn;
    }

    public void StartPriceUpdate() {
        if(objectsOnShelf.Count > 0) {
            UIController.instance.OpenUpdatePrice(info);
        }
    }

    public void UpdateDisplayPrice(float price) {
        if (objectsOnShelf.Count > 0) {
            info.currentPrice = price;
            shelfLabel.text = "$" + info.currentPrice.ToString("F2");
        }
    }
}