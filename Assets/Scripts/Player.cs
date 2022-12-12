using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Player class is responsible moving the player back and forth between cart and shelves and carries products as well.
/// </summary>
public class Player : MonoBehaviour
{
    public static Player instance;
    public bool isMoving;
    public bool closeToProducts;
    public bool loaded;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        StartCoroutine(LerpPosition(new Vector3(2f, transform.position.y, 0f), 3f));
    }

    /// <summary>
    /// Moves the gameObject back and forth with given positions and duration.
    /// </summary>
    /// <param name="targetPosition">Destination of the object to be moved</param>
    /// <param name="duration">How long this duration should take in seconds</param>
    /// <returns>IEnumerator object which holds current coroutine reference at that moment</returns>
    public IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;
        isMoving = true;
        closeToProducts = false;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            
            yield return null;
        }

        transform.position = targetPosition;
        isMoving = false;
        
        if (Mathf.Abs(transform.position.x - 2.0f) < 0.1f)
        {
            closeToProducts = true;
        }
    }

    /// <summary>
    /// Triggers when the selected product collides with Player's collider and sets the product info and attach the product
    /// randomised point on the player.
    /// </summary>
    /// <param name="other">Selected product which has just collided with Player</param>
    private void OnTriggerEnter2D(Collider2D other)  
    {
        Debug.Log("collision "+other.name);
        ShoppingCart.instance.productGameObjects.Add(other.gameObject);
        
        Transform selectedTransform = other.transform;
        selectedTransform.position = transform.position + new Vector3(Random.Range(0f, 2f), Random.Range(0f, 2f), 0f);
        selectedTransform.parent = transform;

        other.gameObject.GetComponent<BoxCollider2D>().enabled = false;

        if (other.gameObject.CompareTag("Food"))
        {
            Product food = new Food(25, "Cake", Product.ProductType.Food);
            ShoppingCart.instance.products.Add(food);
        }
        
        else if (other.gameObject.CompareTag("Book"))
        {
            Product book = new Book(35, "1984", Product.ProductType.Book);
            ShoppingCart.instance.products.Add(book);
        }
        
        else if (other.gameObject.CompareTag("MusicCd"))
        {
            Product musicCd = new Food(150, "Elden Ring Soundtrack", Product.ProductType.MusicCd);
            ShoppingCart.instance.products.Add(musicCd);
        }
        
        else if (other.gameObject.CompareTag("Magazine"))
        {
            Product magazine = new Magazine(375, "Nature", Product.ProductType.Magazine);
            ShoppingCart.instance.products.Add(magazine);
        }
        
        ShoppingCart.instance.productSelected = false;
        loaded = true;
    }
    
}
