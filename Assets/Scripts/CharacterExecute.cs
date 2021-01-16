using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterExecute : MonoBehaviour
{
    //自己创建的类的字段
    protected Movement m_movement;
    protected Jump m_jumpping;

    //public GrapplingHook m_grap;
    public HookedChecker m_hookedCheck;
    public GroundedCheck m_groundCheck;
    public SoundDate m_soundDate ;

    //


    void OnEnable()
    { 
    
    }
        

    void Start()
    {
        //自己创建的类的实例
        m_movement = new Movement();
        m_jumpping = new Jump();
        //m_grap = new GrapplingHook();
        m_hookedCheck = new HookedChecker();
        m_groundCheck = new GroundedCheck();
    }

    void Update()
    {

        //m_grap.Grappling();
        //m_groundCheck.CheckIsGrounded();
        //m_movement.Move(传参);
        m_movement.HorizontalMovement();
        m_soundDate.PlayAudio();

    }

    void FixedUpdate()
    {

    }
}

