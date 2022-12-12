using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// ShoppingCart class is responsible for drag-drop products and store the related info about them and show the cart details section in game.
/// </summary>
public class ShoppingCart : MonoBehaviour
{
    private GameObject selectedObject;
    
    public static ShoppingCart instance;
    public List<Product> products;
    public List<GameObject> productGameObjects;
    public bool productSelected;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
        products = new List<Product>();
    }

    private void Update()
    {
        if (!Player.instance.isMoving)
        {
            if (Player.instance.closeToProducts)
            {
                if (Input.GetMouseButtonDown(0) && !productSelected)
                {
                    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    var hit = Physics2D.GetRayIntersection(ray, 1500f);
                    print($"Mouse is over {hit.collider.name}");

                    if (hit.collider.gameObject.CompareTag("Player") && hit.collider.gameObject.transform.childCount > 0)
                    {
                        StopAllCoroutines();
                        StartCoroutine(Player.instance.LerpPosition(new Vector3(-4f, Player.instance.transform.position.y, 0f), 3f));
                    }

                    else
                    {
                        Vector3 newPosition = hit.transform.position;
                        newPosition += new Vector3(-1f, 0f, 0f);

                        selectedObject = Instantiate(hit.collider.gameObject, newPosition, Quaternion.identity);
                        selectedObject.GetComponent<SpriteRenderer>().sortingOrder = 3;

                        productSelected = true;
                    }
                }

                if (Input.GetMouseButton(0) && productSelected)
                {
                    if (selectedObject && !selectedObject.CompareTag("Player"))
                    {
                        Debug.Log("Dragging!");
                        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - selectedObject.transform.position;
                        selectedObject.transform.Translate(mousePosition);
                    }
                }
            }
            
            else if (Player.instance.loaded)
            {
                PutProductsToCart();
                Player.instance.loaded = false;
                StartCoroutine(Player.instance.LerpPosition(new Vector3(2f, transform.position.y, 0f), 3f));
            }
        }
    }
    
    /// <summary>
    /// Puts selected products to the cards with little randomisation in positions.
    /// </summary>
    private void PutProductsToCart()
    {
        foreach (GameObject child in productGameObjects)
        {
            Debug.Log("Removed the child " + child.name);
            child.transform.parent = null;
            child.transform.position = Player.instance.transform.position + new Vector3(Random.Range(-5f, -1f), Random.Range(-0.2f, 0.2f), 0f);
        }
        
        productGameObjects.Clear();
    }
}
