using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    private CapsuleCollider2D coll;
    [Header("������")]
    public bool manual;
    public Vector2 bottomOffset;
    public Vector2 leftOffset;
    public Vector2 rightOffset;
    //������ⷶΧ
    public float checkRaduis;
    public LayerMask groundLayerl;
    public LayerMask pushLayerl;
    [Header("״̬����")]
    public bool isGround;
    public bool isPush;
    public bool touchLeftWall;
    public bool touchRightWall;
    public bool touchLeftItem;
    public bool touchRightItem;
    private void Awake()
    {
        coll = GetComponent<CapsuleCollider2D>();
    }
    private void Update()
    {
        if (!manual)
        {
            if (transform.localScale.x == 1)
            {
                rightOffset = new Vector2(coll.bounds.size.x / 2 + coll.offset.x, coll.bounds.size.y / 2);
                leftOffset = new Vector2(-rightOffset.x + coll.offset.x * 2, rightOffset.y);
            }
            else if (transform.localScale.x == -1)
            {
                rightOffset = new Vector2(coll.bounds.size.x / 2 - coll.offset.x, coll.bounds.size.y / 2);
                leftOffset = new Vector2(-rightOffset.x - coll.offset.x * 2, rightOffset.y);
            }
        }
        check();
    }
    public void check()
    {
        //��⵽��ײָ��ͼ��᷵��true����false
      isGround= Physics2D.OverlapCircle((Vector2)transform.position+ bottomOffset*transform.localScale, checkRaduis, groundLayerl);
      touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, checkRaduis, groundLayerl);
      touchRightWall= Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, checkRaduis, groundLayerl);
      touchLeftItem = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, checkRaduis, pushLayerl);
      touchRightItem = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, checkRaduis, pushLayerl);
      isPush = touchLeftItem || touchRightItem;
    }
    //�����屻ѡ�л�һֱ�ڴ��ڽ��л���
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset*transform.localScale, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, checkRaduis);
    }
}
