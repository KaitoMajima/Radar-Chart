using System.Collections;
using TMPro;
using UnityEngine;

namespace KaitoMajima.SimpleTextReveal
{
    public class SimpleTextReveal : MonoBehaviour
    {
        
        [Header("Local Dependencies")]
        [SerializeField] private TMP_Text _textComponent;

        [Header("Settings")]

        [SerializeField] private bool _playOnStart;
        [SerializeField] private float _initialDelay;
        [SerializeField] private float _letterDelay = 0.1f;
        private void Start()
        {
            if(_playOnStart)
                Reveal();
        }

        public void Reveal()
        {
            _textComponent.maxVisibleCharacters = 0;

            StartCoroutine(Co_Reveal(_textComponent, _initialDelay, _letterDelay));
        }

        private IEnumerator Co_Reveal(TMP_Text textComponent, float initialDelay, float letterDelay)
        {
            yield return new WaitForSeconds(initialDelay);

            var parsedText = textComponent.GetParsedText();

            while(textComponent.maxVisibleCharacters != parsedText.Length)
            {
                _textComponent.maxVisibleCharacters += 1;
                yield return new WaitForSeconds(letterDelay);
            }
        }
    }
}
