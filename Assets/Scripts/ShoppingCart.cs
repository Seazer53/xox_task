using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// ShoppingCart class is responsible for drag-drop products and store the related info about them and show the cart details section in game.
/// </summary>
public class ShoppingCart : MonoBehaviour
{
    #region Private Fields
    
    // To set selectedObject's parent as player so that selected objects can be easily deleted later.
    private GameObject selectedObject;
    
    private int totalCost;
    private string ingredients;
    private Camera mainCamera;
    
    #endregion

    #region Public Fields

    public List<Product> products;
    public List<GameObject> productGameObjects;
    
    // Just to make sure only selected one product at a time so that it can be dragged-dropped to the player.
    public bool productSelected;
    
    public TMP_Text cartText;
    public TMP_Text totalCostText;

    #endregion

    #region Public Properties

    public static ShoppingCart Instance { get; private set; }

    #endregion

    #region MonoBehaviour Callbacks

    /// <summary>
    /// Caches references so that they can be referenced without looking up.
    /// </summary>
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        
        else 
        { 
            Instance = this; 
        }

        products = new List<Product>();
        mainCamera = Camera.main;
    }

    /// <summary>
    /// Handles the game logic. Input is taken by left mouse click and holding down the left mouse click. When the player
    /// doesn't want any more products, the player sprite needs to be left clicked  to move those products to the cart.
    /// </summary>
    private void Update()
    {
        if (!Player.Instance.isMoving)
        {
            if (Player.Instance.closeToProducts)
            {
                if (Input.GetMouseButtonDown(0) && !productSelected)
                {
                    var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                    var hit = Physics2D.GetRayIntersection(ray, 1500f);

                    if (hit)
                    {
                        if (hit.collider.gameObject.CompareTag("Player") && hit.collider.gameObject.transform.childCount > 0)
                        {
                            StopAllCoroutines();
                            StartCoroutine(Player.Instance.LerpPosition(new Vector3(-4f, Player.Instance.transform.position.y, 0f), 3f));
                        }

                        else if (!hit.collider.gameObject.CompareTag("Player"))
                        {
                            Vector3 newPosition = hit.transform.position;
                            newPosition += new Vector3(-1f, 0f, 0f);

                            selectedObject = Instantiate(hit.collider.gameObject, newPosition, Quaternion.identity);
                            // Just to see better objects in the game, we increase their sorting layer by one.
                            selectedObject.GetComponent<SpriteRenderer>().sortingOrder = 3;

                            productSelected = true;
                        }
                    }
                }

                if (Input.GetMouseButton(0) && productSelected)
                {
                    if (selectedObject && !selectedObject.CompareTag("Player"))
                    {
                        Debug.Log("Dragging!");
                        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) -
                                                selectedObject.transform.position;
                        selectedObject.transform.Translate(mousePosition);
                    }
                }
            }

            else if (Player.Instance.loaded)
            {
                PutProductsToCart();
                CalculateAndUpdateCart();
                Player.Instance.loaded = false;
                StartCoroutine(Player.Instance.LerpPosition(new Vector3(2f, transform.position.y, 0f), 3f));
            }
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Puts selected products to the cards with little randomisation in positions so that products don't fully overlap into each other.
    /// </summary>
    private void PutProductsToCart()
    {
        foreach (GameObject child in productGameObjects)
        {
            Debug.Log("Removed the child " + child.name);
            child.transform.parent = null;
            child.transform.position = Player.Instance.transform.position + new Vector3(Random.Range(-5f, -1f), Random.Range(-0.5f, 0.1f), 0f);
        }

        productGameObjects.Clear();
    }

    /// <summary>
    /// Calculates and updates the related UI to show how much all of these products costs. 
    /// </summary>
    private void CalculateAndUpdateCart()
    {
        foreach (var product in products)
        {
            ingredients += product.Name + ", " + product.Type + " " + product.Price + "TL \n";
            totalCost += product.Price;
        }

        cartText.text = "Ingredients: \n" + ingredients;
        totalCostText.text = "Cost: " + totalCost + " TL";

        products.Clear();
    }

    /// <summary>
    /// Resets the whole previous shopping information.
    /// </summary>
    private void ResetShopping()
    {
        cartText.text = "Ingredients: ";
        totalCostText.text = "Cost: ";
        ingredients = "";
        products.Clear();
        productGameObjects.Clear();

        // Find all of the products on the scene by filtering with tags.
        var books = GameObject.FindGameObjectsWithTag("Book").ToList();
        var foods = GameObject.FindGameObjectsWithTag("Food").ToList();
        var musicCds = GameObject.FindGameObjectsWithTag("MusicCd").ToList();
        var magazines = GameObject.FindGameObjectsWithTag("Magazine").ToList();
        
        // Then combine all of them into one list so that we can find and destroy easily.
        var all = books.Concat(foods.Concat(musicCds.Concat(magazines))).ToList();

        foreach (var product in all)
        {
            if (product.transform.parent == null)
            {
                Destroy(product);
            }
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Prints invoice to the .txt file. File can be found in AppData/LocalLow/DefaultCompany/ShippingCartInvoice.txt
    /// </summary>
    public void PrintInvoice()
    {
        string fileName = "invoice.txt";
        string dataPath = "Assets/Output/";

        string invoice = "INVOICE \n" + cartText.text + totalCostText.text + "\n";


        if (!Directory.Exists(dataPath))
        {
            Directory.CreateDirectory(dataPath);
        }

        dataPath += fileName;

        try
        {
            File.AppendAllText(dataPath, invoice);
            Debug.Log("Appending invoice...");
            ResetShopping();
            
            string readText = File.ReadAllText(dataPath);
            Debug.Log(readText);
        }

        catch (Exception ex)
        {
            string ErrorMessages = "File Write Error\n" + ex.Message;
            Debug.LogError(ErrorMessages);
        }
    }

    #endregion
}
