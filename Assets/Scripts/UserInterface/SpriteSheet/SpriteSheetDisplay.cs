using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Agate.WaskitaInfra1.UserInterface.SpriteSheet
{
    public class SpriteSheetDisplay : MonoBehaviour
    {
        [SerializeField]
        private float _speed;

        [SerializeField]
        private int _frameRate = 30;

        [SerializeField]
        private bool _loop;

        private Image _image = null;
        public Sprite[] _sprites;
        private float _timePerFrame = 0f;
        private float _elapsedTime = 0f;
        private int _currentFrame = 0;

        private void Awake()
        {
            _image = GetComponent<Image>();
            enabled = false;
        }

        public void LoadSpriteSheet(Sprite[] dataSprites, bool isLoop, float speed)
        {
            if (dataSprites != null && dataSprites.Length > 0)
            {
                StoreSprites(dataSprites);
                _loop = isLoop;
                _speed = speed;
                _timePerFrame = 1f / _frameRate;
                Play();
            }
            else
            {
                Debug.Log("failed to load sprite sheet");
            }
        }

        private void StoreSprites(Sprite[] dataSprites)
        {
            _sprites = new Sprite[dataSprites.Length];
            for(int i = 0; i < dataSprites.Length; i++)
            {
                _sprites[i] = dataSprites[i];
            }
        }

        private void Play()
        {
            enabled = true;
        }

        void Update()
        {
            _elapsedTime += Time.deltaTime * _speed;
            if (_elapsedTime >= _timePerFrame)
            {
                _elapsedTime = 0;
                ++_currentFrame;
                SetSprite();
                if (_currentFrame >= _sprites.Length)
                {
                    if (_loop)
                    {
                        _currentFrame = 0;
                    }
                    else
                    {
                        enabled = false;
                    }

                }
            }
            
        }

        private void SetSprite()
        {
            if (_currentFrame >= 0 && _currentFrame < _sprites.Length)
            {
                _image.sprite = _sprites[_currentFrame];
            }
        }
    }
}

