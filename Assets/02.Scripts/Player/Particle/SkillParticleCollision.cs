using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillParticleCollision : MonoBehaviour
{
    public enum Type
    {
        Stun,
        Fire,
    }

    public Type type;

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Enemy"))
        {
            switch (type)
            {
                case Type.Stun:
                    if (other.TryGetComponent<BossCollider>(out BossCollider bossCollider))
                    {
                        int index = bossCollider.idx;
                        other.transform.GetComponentInParent<Boss>().Stun(index, Random.Range(1f,3f));
                    }
                    else if (other.TryGetComponent<Monster>(out Monster monster))
                    {
                        if (monster.isCanDamage)
                            monster.skillDamage(Random.Range(1f,3f));
                    }
                    break;

                case Type.Fire:

                    if (other.TryGetComponent<BossCollider>(out BossCollider bossCollider_))
                    {
                        int index = bossCollider_.idx;
                        Boss boss = other.transform.GetComponentInParent<Boss>();
                        boss.Damage(index, boss.curHp * Random.Range(0.01f, 0.02f));
                    }
                    else if (other.TryGetComponent<Monster>(out Monster monster_))
                    {
                        if (monster_.isCanDamage)
                            monster_.skillDamage(monster_.curHp * Random.Range(0.03f, 0.05f));
                    }
                    break;

                default:

                    break;
            }
        }
    }
}
