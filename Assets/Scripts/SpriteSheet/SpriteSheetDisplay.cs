using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Agate.SpriteSheet
{
    public class SpriteSheetDisplay : MonoBehaviour
    {
        [SerializeField]
        private int _frameRate = 30;

        [SerializeField]
        private bool _loop;

        [SerializeField]
        private Image _image = null;

        private List<Sprite> _sprites;
        private float _elapsedTime = 0f;
        private int _currentFrame = 0;


        private void SetSpriteSheet(IEnumerable<Sprite> dataSprites, bool isLoop, int framerate)
        {
            if (dataSprites == null)
                throw new Exception("failed to load sprite sheet");
            _sprites = new List<Sprite>(dataSprites);
            _loop = isLoop;
            _frameRate = framerate;
            _currentFrame = 0;
            if (_sprites.Count < 1) return;
            _image.sprite = _sprites[0];
        }

        public void Clear()
        {
            _sprites.Clear();
        }

        public void SetSpriteSheet(ISpriteSheet spriteSheet)
        {
            SetSpriteSheet(spriteSheet.Sprites, spriteSheet.Loop, spriteSheet.Framerate);
        }

        private void Update()
        {
            if (_sprites == null || _sprites.Count < 1) return;
            if (_currentFrame >= _sprites.Count && !_loop) return;

            _elapsedTime += Time.deltaTime;
            if (!(_elapsedTime >= 1f / _frameRate)) return;

            CycleSprite();
            _elapsedTime = 0;
        }

        private void CycleSprite()
        {
            _currentFrame++;
            if (_currentFrame >= _sprites.Count && _loop)
                _currentFrame = 0;
            if (_currentFrame >= _sprites.Count) return;
            _image.sprite = _sprites[_currentFrame];
        }
    }

    public interface ISpriteSheet
    {
        IEnumerable<Sprite> Sprites { get; }
        int Framerate { get; }
        bool Loop { get; }
    }
}