using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public int coin = 0;

    public TMP_Text coinText;
    public float moveSpeed;
    public float maxVelocity;
    public float rayDistance;
    public LayerMask isgroundedLayer;
    public float jumpForce;
    
    
    private GameControl _gameControl;
    private PlayerInput _playerInput;
    private Camera _mainCamera;
    private Rigidbody _rigidbody;

    private Vector2 _moveInput;
    [SerializeField] private bool _isGrounded;

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
        // Calcula o movimento no eixo da camera para o movimento frente/tras
        Vector3 moveVertical = _mainCamera.transform.forward * _moveInput.y;
        
        // Calcula o movimento no eixo da camera para o movimento esquerda/direita
        Vector3 moveHorizontal = _mainCamera.transform.right * _moveInput.x;
        
        // Adiciona a força mo objeto através do rigidybody, com intensidade definida por movespeed
        _rigidbody.AddForce((moveVertical + moveHorizontal) * moveSpeed * Time.fixedDeltaTime);
    }

    private void FixedUpdate()
    {
        Move();
        LimitVelocity();
    }

    private void LimitVelocity()
    {
        // Pegar a velocidade do player
        Vector3 velocity = _rigidbody.velocity;

        ;
        // Checar se a velocidade esta dentro dos limites nos diferentes eixos 
        // Limitando o eixo x usando ifs, Abs e Sign
        if (Mathf.Abs(velocity.x) > maxVelocity) velocity.x = maxVelocity;
        
        // -maxVelocity < velocity.z < maxVelocity
        velocity.z = Mathf.Clamp(velocity.z, -maxVelocity, maxVelocity);
        
        // Alterar a velocidade do player para ficar dentro dos limites
        _rigidbody.velocity = velocity;
    }
    
        /* COMO FAZER O JOGADOR PULAR
        // 1 - Checar se o jogador está no chão
        // -- a - checar colisão a partir do fisico (usando os eventos de colisão)
        // -- a - vantagem: fácil de implementar (adicionar uso função que ja existe no unity - OnCollision)
        // -- a - desvantagem: nao sabemos a hora exata que o unity vai chamar essa funcao (pode ser que o jogador toque no chão e demores alguns frames pro jogo saber que voce esta no chão)
        // * -- b - atraves do raycast: o ---/ bolinha vai atrair um raio, o raio vai bater em algum objeto e receber o resultado dessa colisão
        // * -- b - podemos usar layers pra definir quais objetos que o raycast deve checar a colisão
        // + -- b - vantagens: rsposta da colisao é imediata
        // 2- jogador precisa apertar o botao de pular 
        */
        
        private void Jump()
        {
            if (_isGrounded) _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        private void CheckGround()
        { 
            _isGrounded = Physics.Raycast(origin: transform.position, direction: Vector3.down, rayDistance, isgroundedLayer);
        }
        
        private void Update()
        {
            CheckGround();
        }

        private void OnDrawGizmos()
        {
            Debug.DrawRay(start:transform.position, dir:Vector3.down * rayDistance, Color.yellow);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Coin"))
            {
                coin++;
                coinText.text = coin.ToString();
                Destroy(other.gameObject);
            }
        }
}
     
