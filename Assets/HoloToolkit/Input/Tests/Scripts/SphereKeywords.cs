// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;

namespace HoloToolkit.Unity.InputModule.Tests
{
    [RequireComponent(typeof(Renderer))]
    public class SphereKeywords : MonoBehaviour, ISpeechHandler
    {
        private Material cachedMaterial;

        private void Awake()
        {
            cachedMaterial = GetComponent<Renderer>().material;
        }

		public void CloseObject(GameObject myObject)
		{
			Destroy(myObject);
		}

        public void ChangeColor(string color)
        {
            switch (color)
            {
				case "close":
					CloseObject(gameObject);
					break;
				case "red":
                    cachedMaterial.SetColor("_Color", Color.red);
                    break;
                case "blue":
                    cachedMaterial.SetColor("_Color", Color.blue);
                    break;
                case "green":
                    cachedMaterial.SetColor("_Color", Color.green);
                    break;
				case "yellow":
					cachedMaterial.SetColor("_Color", Color.yellow);
					break;
			}
        }

        public void OnSpeechKeywordRecognized(SpeechKeywordRecognizedEventData eventData)
        {
            ChangeColor(eventData.RecognizedText.ToLower());
        }

        private void OnDestroy()
        {
            DestroyImmediate(cachedMaterial);
        }
    }
}