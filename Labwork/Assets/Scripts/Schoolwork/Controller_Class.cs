using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Schoolwork
{
    public class Controller_Class : MonoBehaviour
    {

        PlayerInputs input;
        Character_Class player;

        private void Awake()
        {
            input = new PlayerInputs();
        }

        private void OnEnable()
        {
            input.Enable();
        }

        private void OnDisable()
        {
            input.Disable();
        }


        // Start is called before the first frame update
        void Start()
        {
            player = GetComponent<Character_Class>();
            input.Player.Move.performed += ctx => player.MovePlayer(ctx);
            input.Player.Move.canceled += ctx => player.MovePlayer(ctx);
            input.Player.Fire.performed += ctx => player.Fire(ctx);
            input.Player.Jump.performed += ctx => player.Jump(ctx);

        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }  
}

