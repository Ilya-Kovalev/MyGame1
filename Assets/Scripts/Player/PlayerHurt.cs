using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerHurt : MonoBehaviour
{
    private const string IsHurt = "IsHurt";
    private const string IsDying = "IsDying";

    [SerializeField] private Player _player;
    [SerializeField] private Image _image;
    [SerializeField] private UnityEvent _hurt;

    private Animator _animator;
    private float _recoveryTime = 2;
    private int _dyingTime = 2;
    private int _playerLayer = 8;
    private int _enemyLayer = 6;
    private int _damage;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.SetBool(IsHurt, false);
        _animator.SetBool(IsDying, false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<DeathZone>(out DeathZone deathzone)) 
        {
            _hurt.Invoke();
            _damage = deathzone.GetInfo();
            _player.ChangeHealth(_damage);

            if(_player.GetHealth() > 0) 
            {
                StartCoroutine(WaitingRecovery());
            }
            else if(_player.GetHealth() == 0) 
            {
                _animator.SetBool(IsDying, true);
                StartCoroutine(PlayerDestroying());
            }
        }
    }

    private IEnumerator WaitingRecovery() 
    {
        _animator.SetBool(IsHurt, true);
        Physics2D.IgnoreLayerCollision(_playerLayer, _enemyLayer);
        yield return new WaitForSeconds(_recoveryTime);
        _animator.SetBool(IsHurt, false);
        Physics2D.IgnoreLayerCollision(_playerLayer, _enemyLayer, false);
    }

    private IEnumerator PlayerDestroying()
    {
        
        Physics2D.IgnoreLayerCollision(_playerLayer, _enemyLayer);
        _player.GetComponent<PlayerMovement>().enabled = false;
        yield return new WaitForSeconds(_dyingTime);
        Destroy(_player.transform.gameObject);
        _image.gameObject.SetActive(true);
    }
}
