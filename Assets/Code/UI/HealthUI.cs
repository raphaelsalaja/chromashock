using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;
public class HealthUI : MonoBehaviour
{

    public Animator transisition;
    public Menu menu;
    public LevelGenerator levelGenerator;
    [Header("HEALTH")]
    [Space]
    public Image HP_FX_IMAGE, HP_IMAGE;
    public Text HP_TEXT, HP_TEXT_SHADOW;
    public static float HP = 5;
    public static float HP_MAX = 5;
    public float HP_SPEED = 0.0005f;


    [Header("WORLD")]
    [Space]
    public Text WORLD_LEVEL_TEXT;
    public static int stage = 0;
    public static int world = 1;

    [Header("ENEMIES")]
    public int enemiesCount;
    public Text enemies_left;
    public Text enemies_left_shadow;

    [Header("WEAPONS")]
    [Space]
    public Text AMMO_TEXT, AMMO_TEXT_SHADOW;
    public static int ammoMax = 32;
    public int ammo = 32;
    public static int reloadTime = 1;


    [Header("LEVEL TYPE")]
    [Space]
    public bool endless;


    private void Start()
    {
        if (endless)
        {
            HP_MAX = 1;
        }
        else
        {
            HP = HP_MAX;
        }
    }
    private void Update()
    {
        enemiesCount = levelGenerator.EnemyCount;
        Health();
        Ammo();
        Enemies();

        HP_TEXT_SHADOW.text = HP_TEXT.text;
        AMMO_TEXT_SHADOW.text = AMMO_TEXT.text;


        if (HP <= 0)
        {
            Die();
        }
        if (world == 3 && !endless)
        {
            Win();
        }
    }

    public void Enemies()
    {
        enemies_left.text = enemiesCount.ToString();
        enemies_left_shadow.text = enemies_left.text;
    }

    public void World()
    {
        stage++;
        if (stage == 3)
        {
            stage = 1;
            world++;
        }

        WORLD_LEVEL_TEXT.text = world + " - " + stage;
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


    public void Die()
    {
        StartCoroutine(LoadLevel(3));
    }

    public void Win()
    {
        StartCoroutine(LoadLevel(4));
    }

    private IEnumerator LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);

        yield return new WaitForSeconds(1);

        transisition.SetTrigger("Start");
    }
}
