using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    
    private GameControl _gameControl;
    private PlayerInput _playerInput;
    private Camera _mainCamera;
    private Rigidbody _rigidbody;

    private Vector2 _moveInput;

    private void OnEnable()
    {
        // inicializacao de variavel
        _gameControl = new GameControl();
        
        // referencias dos componentes no mesmo objeto na unity
        _playerInput = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody>();
        
        // referencia para a camera main guardada na classe camera
        _mainCamera = Camera.main;
        
        // atribuindo ao delegate do action triggered no player input
        _playerInput.onActionTriggered += OnActionTriggered;

    }

    private void OnDisable()
    {
        _playerInput.onActionTriggered -= OnActionTriggered;
    }

    private void OnActionTriggered(InputAction.CallbackContext obj)
    {
        // comecando o nome do action que este chegando com o nome do action de moviment
        if(obj.action.name.CompareTo(_gameControl.Gameplay.Moviment.name) == 0)
        {
            // atribuir ao moveimput o valor proveniente ao input do jogador com um vector2
            _moveInput = obj.ReadValue<Vector2>();
        }
    }
    
    private void Move()
    {
        // calcula o movimento no eixo da camera para o movimento frente/tras
        Vector3 moveVertical = _mainCamera.transform.forward * _moveInput.y;
        
        // calcula o movimento no eixo da camera para o movimento esquerda/direita
        Vector3 moveHorizontal = _mainCamera.transform.right * _moveInput.x;
        
        // adiciona a força mo objeto através do rigidybody, com intensidade definida por movespeed
        _rigidbody.AddForce((moveVertical + moveHorizontal) * moveSpeed * Time.fixedDeltaTime);
    }

    private void FixedUpdate()
    {
        Move();
    }
}
     
