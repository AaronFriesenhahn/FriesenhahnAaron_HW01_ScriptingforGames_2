﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slower : Enemy
{
    [SerializeField] float _speedAmount = 1;
    [SerializeField] public float _PowerUpDuration = 2;
    [SerializeField] AudioClip _powerUpSound;

    float _originalMoveSpeed;
    float _originalPowerUpDuration;
    int x = 0;

    protected override void PlayerImpact(Player player)
    {
        //pull motor controller from the player
        TankController controller = player.GetComponent<TankController>();

        //set _originalMoveSpeed only once
        if (x == 0)
        {
            _originalMoveSpeed = controller.MoveSpeed;
        }

        _originalPowerUpDuration = _PowerUpDuration;
        if (controller != null)
        {
            x = 1;
            if (controller.MoveSpeed > 0)
            {
                controller.MoveSpeed -= _speedAmount;
            }
            if (controller.MoveSpeed < 0)
            {
                controller.MoveSpeed = 0.02f;
            }
            //timer that restores the player to their normal speed
            IEnumerator PowerUpCountdown()
            {
                while (_PowerUpDuration > 0)
                {
                    yield return new WaitForSeconds(1f);

                    _PowerUpDuration--;
                }

                if (_PowerUpDuration <= 0)
                {
                    if (_powerUpSound != null)
                    {
                        AudioHelper.PlayClip2D(_powerUpSound, 1f);
                    }
                    _PowerUpDuration = 0;
                    controller.MoveSpeed = _originalMoveSpeed;
                    _PowerUpDuration = _originalPowerUpDuration;
                    x = 0;
                }
            }
            //call PowerDown funtion after PowerUp duration is over and disable game object
            StartCoroutine(PowerUpCountdown());
        }
    }
}
