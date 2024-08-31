using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
    public static event Action<CardObject> onBeginDrag;
    public static event Action onEndDrag;
    public static event Action onPointerEnter;
    public static event Action onPointerExit;
    public static event Action onPointerUp;
    public static event Action onPointerDown;

    bool isDragging = false;
    private Vector3 offset;
    [SerializeField] private float moveSpeedLimit = 50;

    [SerializeField] private GraphicRaycaster canvasRaycaster;

    void Update()
    {
        ClampPosition();

        if (isDragging)
        {
            DragCard();
        }
    }

    private void DragCard()
    {
        Vector2 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - offset;
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        Vector2 velocity = direction * Mathf.Min(moveSpeedLimit, Vector2.Distance(transform.position, targetPosition) / Time.deltaTime);
        transform.Translate(velocity * Time.deltaTime);
    }

    void ClampPosition()
    {
        Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -screenBounds.x, screenBounds.x);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, -screenBounds.y, screenBounds.y);
        transform.position = new Vector3(clampedPosition.x, clampedPosition.y, 0);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        onBeginDrag?.Invoke(this);
        isDragging = true;

        // get the offset position
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset = mousePosition - (Vector2)transform.position;

        // disable the canvas raycaster so it doesn't accept other input
        canvasRaycaster.enabled = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        onEndDrag?.Invoke();
        isDragging = false;

        canvasRaycaster.enabled = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        onPointerDown?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        onPointerUp?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onPointerEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onPointerExit?.Invoke();
    }
}
