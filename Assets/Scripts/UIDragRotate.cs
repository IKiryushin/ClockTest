using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragRotate : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public AnalogueClock _analogueClock;

    private Vector2 position;

    private void Start()
    {
        position = new(transform.position.x, transform.position.y);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!ClockManager.overrideTime)
            return;

        _analogueClock.handIsDraged = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!ClockManager.overrideTime)
            return;

        Vector2 Rpos = position - eventData.position; ;
        float angle = Vector2.Angle(Vector2.up, -Rpos);

        if (Rpos.x < 0)
            angle = -angle;


        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!ClockManager.overrideTime)
            return;

        _analogueClock.OnDragEnd();
    }
}
