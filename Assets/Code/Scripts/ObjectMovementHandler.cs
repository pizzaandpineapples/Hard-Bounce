using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ObjectMovementHandler: MonoBehaviour
{
    //[SerializeField] private float moveSpeed;
    //[SerializeField] private float moveDirection;
    //[SerializeField] private float moveDistance;

    //[SerializeField] private float moveStartValue;
    //[SerializeField] private float moveEndValue;
    //[SerializeField] private float moveDuration;
    //[SerializeField] private float moveTimeGap;

    [SerializeField] private Vector3 moveStartValue;
    [SerializeField] private Vector3 moveEndValue;
    [SerializeField] private float moveDuration;
    [SerializeField] private float moveTimeGap;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MoveCoroutine());
    }

    //IEnumerator MoveCoroutine()
    //{
    //    transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
    //    yield return new WaitForSeconds(moveTime);
    //    transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
    //    yield return new WaitForSeconds(moveTime);
    //    StartCoroutine(MoveCoroutine());
    //}

    //IEnumerator MoveCoroutine()
    //{
    //    transform.DOMoveY(moveEndValue, moveDuration, false);
    //    yield return new WaitForSeconds(moveTimeGap);
    //    transform.DOMoveY(moveStartValue, moveDuration, false);
    //    yield return new WaitForSeconds(moveTimeGap);
    //    StartCoroutine(MoveCoroutine());
    //}

    IEnumerator MoveCoroutine()
    {
        transform.DOMove(moveEndValue, moveDuration, false).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(moveTimeGap);
        transform.DOMove(moveStartValue, moveDuration, false).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(moveTimeGap);
        StartCoroutine(MoveCoroutine());
    }
}
