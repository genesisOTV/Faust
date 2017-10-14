using UnityEngine;

public class LightsScript : MonoBehaviour {

	public bool areLightsOn = false;
	[Range(0, 1)] public float BackgroundLightIntensity = 0.25f;
	public int NumberOfLights = 1;
	public GameObject LightPrefab;
	Light[] myLights;

	void Awake() {
		//Create lights
		myLights = new Light[NumberOfLights];
		for (int i = 0; i < NumberOfLights; i++) {
			var newLight = Instantiate(LightPrefab, transform);
			newLight.name = "Light " + i;
			myLights[i] = newLight.gameObject.GetComponent<Light>();
			myLights[i].intensity = 1 - BackgroundLightIntensity;
			myLights[i].gameObject.SetActive(areLightsOn);
		}

		//Initialize ambient light
		RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
		if (areLightsOn) {
			RenderSettings.ambientLight = new Color(BackgroundLightIntensity, BackgroundLightIntensity, BackgroundLightIntensity);
		}
		else {
			RenderSettings.ambientLight = Color.white;
		}
	}

	void Update() {
		//Change ambient light intensity
		if (areLightsOn) {
			RenderSettings.ambientLight = new Color(BackgroundLightIntensity, BackgroundLightIntensity, BackgroundLightIntensity);
		}
		else {
			RenderSettings.ambientLight = Color.white;
		}

		//Activate or deactivate lights and set intensity
		foreach (Light light in myLights) {
			light.intensity = 1 - RenderSettings.ambientLight.grayscale;
			light.gameObject.SetActive(areLightsOn);
		}
	}
}