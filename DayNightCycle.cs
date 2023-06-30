using UnityEngine;
using UnityEditor;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] Material skybox; // the one we are gonna change
    [SerializeField] Material nightSkybox;
    [SerializeField] Material daySkybox;

    Material targetSkybox;

    [SerializeField] Light sun;



    //float speed = 0.1f; // 0.015f

    float distance;
    float dayDuration = 50;
    float speed;

    private void Start()
    {
        targetSkybox = daySkybox;
        distance = Vector4.Distance(nightSkybox.GetColor("_SkyGradientTop"), daySkybox.GetColor("_SkyGradientTop"));

        speed = distance / dayDuration;
    }

    void Update()
    {
        if (CheckingSkybox(nightSkybox))
        {
            targetSkybox = daySkybox;
        }

        if (CheckingSkybox(daySkybox))
        {
            targetSkybox = nightSkybox;
        }

        SetColor("_SunDiscColor");
        //SetFloat("_SunDiscMultiplier", 0);
        //SetFloat("_SunDiscExponent", 1);

        SetColor("_SunHaloColor");
        //SetFloat("_SunHaloExponent", 2);
        //SetFloat("_SunHaloContribution", 3);

        SetColor("_HorizonLineColor");
        //SetFloat("_HorizonLineExponent", 4);
        //SetFloat("_HorizonLineContribution", 5);

        SetColor("_SkyGradientTop");
        SetColor("_SkyGradientBottom");
        //SetFloat("_SkyGradientExponent", 6);

        SunRotation();

        ChangingSkyboxAfterQuitting();
    }

    void SunRotation()
    {
        //float value = 180 / ((targetSkybox.GetColor("_SkyGradientTop").r + targetSkybox.GetColor("_SkyGradientTop").g + targetSkybox.GetColor("_SkyGradientTop").b) / 3
        //    - (skybox.GetColor("_SkyGradientTop").r + skybox.GetColor("_SkyGradientTop").g + skybox.GetColor("_SkyGradientTop").b) / 3);
        //print(value);
        //sun.transform.eulerAngles = new Vector3(sun.transform.eulerAngles.x + value, 0, 0);

        //float distance = Vector4.Distance(targetSkybox.GetColor("_SkyGradientTop"), skybox.GetColor("_SkyGradientTop"));
        //float time = 1;

        //sun.transform.Rotate(speed, 0, 0);
    }

    void SetColor(string name)
    {
        skybox.SetColor(name, Vector4.MoveTowards(skybox.GetColor(name), targetSkybox.GetColor(name), speed * Time.deltaTime));
    }

    bool CheckColor(string name, Material target)
    {
        return skybox.GetColor(name) == target.GetColor(name);
    }

    void SettingDefaultValues()
    {
        skybox.SetColor("_SunDiscColor", daySkybox.GetColor("_SunDiscColor"));
        skybox.SetFloat("_SunDiscMultiplier", daySkybox.GetFloat("_SunDiscMultiplier"));
        skybox.SetFloat("_SunDiscExponent", daySkybox.GetFloat("_SunDiscExponent"));

        skybox.SetColor("_SunHaloColor", daySkybox.GetColor("_SunHaloColor"));
        skybox.SetFloat("_SunHaloExponent", daySkybox.GetFloat("_SunHaloExponent"));
        skybox.SetFloat("_SunHaloContribution", daySkybox.GetFloat("_SunHaloContribution"));

        skybox.SetColor("_HorizonLineColor", daySkybox.GetColor("_HorizonLineColor"));
        skybox.SetFloat("_HorizonLineExponent", daySkybox.GetFloat("_HorizonLineExponent"));
        skybox.SetFloat("_HorizonLineContribution", daySkybox.GetFloat("_HorizonLineContribution"));

        skybox.SetColor("_SkyGradientTop", daySkybox.GetColor("_SkyGradientTop"));
        skybox.SetColor("_SkyGradientBottom", daySkybox.GetColor("_SkyGradientBottom"));
        skybox.SetFloat("_SkyGradientExponent", daySkybox.GetFloat("_SkyGradientExponent"));
    }

    bool CheckingSkybox(Material target)
    {
        return CheckColor("_SunDiscColor", target) &&
        CheckColor("_SunHaloColor", target) &&
        CheckColor("_HorizonLineColor", target) &&
        CheckColor("_SkyGradientTop", target) &&
        CheckColor("_SkyGradientBottom", target);
    }

    void ChangingSkyboxAfterQuitting()
    {
        if (!EditorApplication.isPlayingOrWillChangePlaymode &&
      EditorApplication.isPlaying)
        {
            SettingDefaultValues();
        }
    }
}


//float MoveTowards(float current, float target, float step)
//{
//    //float value = current + step;
//    //value = Mathf.Clamp(value, 0, target);
//    //return value;

//    float num = target - current;
//    float num5 = num * num;
//    if (num5 == 0f || (step >= 0f && num5 <= step * step))
//    {
//        return target;
//    }

//    float num6 = (float)Mathf.Sqrt(num5);
//    return current + num / num6 * step;
//}

//void SetFloat(string name)
//{
//    skybox.SetFloat(name, MoveTowards(skybox.GetFloat(name), targetSkybox.GetFloat(name), speed * Time.deltaTime));
//}

//bool CheckFloat(string name, Material target)
//{
//    return skybox.GetFloat(name) == target.GetFloat(name);
//}
