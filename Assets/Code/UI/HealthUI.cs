using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthUI : MonoBehaviour
{

    public LevelGenerator levelGenerator;
    [Header("HEALTH")]
    public Image HP_FX_IMAGE, HP_IMAGE;
    public Text HP_TEXT, HP_TEXT_SHADOW;
    public static float HP = 5;
    public static float HP_MAX = 5;
    public float HP_SPEED = 0.0005f;


    [Header("WORLD")]
    public Text WORLD_LEVEL_TEXT;
    public static int stage, world;


    [Header("ENEMIES")]
    public int enemiesCount;
    public Text enemies_left;
    public Text enemies_left_shadow;

    [Header("WEAPONS")]
    public Text AMMO_TEXT, AMMO_TEXT_SHADOW;
    public static int ammoMax = 32;
    public int ammo = 32;
    public static int reloadTime = 1;



    private void Start()
    {
        HP = HP_MAX;
        enemiesCount = levelGenerator.EnemyCount;
    }
    private void Update()
    {
        Health();
        Ammo();
        Enemies();
        HP_TEXT_SHADOW.text = HP_TEXT.text;
        AMMO_TEXT_SHADOW.text = AMMO_TEXT.text;

    }

    public void Enemies()
    {
        enemies_left.text = enemiesCount.ToString();
        enemies_left_shadow.text = enemies_left.text;
    }

    public void World()
    {
        WORLD_LEVEL_TEXT.text = world + " - " + stage;
        stage++;
        if (stage == 10)
        {
            stage = 1;
            world++;
        }
    }

    public void Ammo()
    {
        AMMO_TEXT.text = ammo.ToString();
        AMMO_TEXT_SHADOW.text = AMMO_TEXT.text;
        if (ammo <= 0)
        {
            StartCoroutine(Reload(reloadTime));

        }

    }

    private IEnumerator Reload(int reloadTime)
    {

        yield return new WaitForSeconds(reloadTime);
        ammo = ammoMax;


    }

    public void ReduceAmmo()
    {
        ammo--;
    }

    public void Health()
    {
        HP_TEXT.text = HP + " / " + HP_MAX;

        HP_IMAGE.fillAmount = HP / HP_MAX;

        Debug.Log(HP_IMAGE.fillAmount);
        if (HP_FX_IMAGE.fillAmount > HP_IMAGE.fillAmount)
        {
            HP_FX_IMAGE.fillAmount -= HP_SPEED;
        }
        else
        {
            HP_FX_IMAGE.fillAmount = HP_IMAGE.fillAmount;
        }

        if (HP > HP_MAX)
        {
            HP = HP_MAX;
        }

    }
    public void TakeDamage(int damage)
    {
        HP -= damage;
    }

}
