using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewPage : MonoBehaviour {
    public Scrollbar m_Scrollbar;
    public ScrollRect m_ScrollRect;

    private float mTargetValue;
    private bool mNeedMove = false;//是否开始移动
    private const float MOVE_SPEED = 1f;
    private const float SMOOTH_TIME = 0.2f;
    private float mMoveSpeed = 0f;

    //计时器起始值
    private float currentTime = 0f;

    public bool autoPlay;//是否自动轮播
    public float currentPage;//轮播起始显示页
    public float countPage;//轮播总页数
    public float startPage;//轮播起始页
    public float endPage;//轮播结束页
    public float intervalTime;//轮播间隔时间


    private bool isMoved = false;//是否移动完成单程
    private float downScorllBarValue;//鼠标按下时ScorllBar的Value值

    /// <summary>
    /// 鼠标按下时，检测当前的ScrollBar的值
    /// </summary>
    public void OnPointerDown()
    {
        downScorllBarValue = m_Scrollbar.value;
        Debug.Log(downScorllBarValue);
    }

    /// <summary>
    /// 当鼠标松开时，根据差值的正负值，确定是向那个方向移动
    /// </summary>
    public void OnPointerUp()
    {
        //倒向拖动鼠标后松开
        if (m_Scrollbar.value - downScorllBarValue < -0.01f)
        {
            if (currentPage >= startPage)
            {
                AutoMove(false);
            }
            else
            {
                currentPage = startPage;
                m_Scrollbar.value = 1 / (countPage - 1) * (startPage - 1);
            }
            if (currentPage == startPage || m_Scrollbar.value == 1 / (countPage - 1) * (startPage - 1))
            {
                isMoved = !isMoved;
            }
        }
        //正向拖动鼠标后松开
        else if (m_Scrollbar.value - downScorllBarValue > 0.01f)
        {
            if (currentPage <= endPage)
            {
                AutoMove(true);
            }
            else
            {
                currentPage = endPage;
                m_Scrollbar.value = 1 / (countPage - 1) * (endPage - 1);
            }
            if (currentPage == endPage || m_Scrollbar.value == 1 / (countPage - 1) * (endPage - 1))
            {
                isMoved = !isMoved;
            }

        }
        mMoveSpeed = 0;
    }

    /// <summary>
    /// 轮播动画函数
    /// </summary>
    /// <param name="isPlayForward">是否正向轮播</param>
    private void AutoMove(bool isPlayForward)
    {
        if (isPlayForward)
        {
            currentPage += 1;
        }
        else
        {
            currentPage -= 1;
        }
        mTargetValue = 1 / (countPage - 1) * (currentPage - 1);//获得下一张播放的目标值
        mNeedMove = true;
        currentTime = 0f;
    }

    void Start()
    {
        m_Scrollbar.value = 1 / (countPage - 1) * (currentPage - 1);//初始化轮播起始页显示
    }


    void Update()
    {

        if (mNeedMove)
        {
            if (Mathf.Abs(m_Scrollbar.value - mTargetValue) < 0.01f)
            {
                m_Scrollbar.value = mTargetValue;
                mNeedMove = false;
                return;
            }
            m_Scrollbar.value = Mathf.SmoothDamp(m_Scrollbar.value, mTargetValue, ref mMoveSpeed, SMOOTH_TIME);
        }

        //如果开启自动轮播
        if (autoPlay)
        {
            currentTime += Time.deltaTime;
            if (intervalTime - currentTime < 0.01f)
            {

                if (!isMoved)
                {
                    //正向轮播
                    if (currentPage <= endPage)
                    {
                        AutoMove(true);
                    }
                    else
                    {
                        currentPage = endPage;
                        m_Scrollbar.value = 1 / (countPage - 1) * (endPage - 1);
                    }
                    //检测正向轮播是否完成，完成则就又开始反向播放
                    if (currentPage == endPage || m_Scrollbar.value == 1 / (countPage - 1) * (endPage - 1))
                    {
                        isMoved = !isMoved;
                    }
                }
                else
                {
                    //反向轮播
                    if (currentPage >= startPage)
                    {
                        AutoMove(false);
                    }
                    else
                    {
                        currentPage = startPage;
                        m_Scrollbar.value = 1 / (countPage - 1) * (startPage - 1);
                    }
                    //检测反向轮播是否完成，完成则就又开始正向播放
                    if (currentPage == startPage || m_Scrollbar.value == 1 / (countPage - 1) * (startPage - 1))
                    {
                        isMoved = !isMoved;
                    }
                }
            }
        }

    }
}
