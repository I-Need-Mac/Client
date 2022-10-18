using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillData
{
    public int skillID;             //��ų ���̵�
    public int coolTime;            //��Ÿ��
    public int atkDis;              //��Ÿ�
    public int projectileCount;     //����ü ����
    public int damage;              //����
    public int speed;               //����ü ���ǵ�
    public int splashRange;         //���� ������
    public int projectileSizeMulti; //����ü ������ ����
    public int skillEffectParam;    //��ų ȿ�� �Ķ�

    public string name;     //��ų �̸�
    public string desc;     //��ų ����
    public string icon;     //������
    public string cutDire;  //��ų �ƾ� ���
    public string skillImg; //��ų �̹���

    public bool skillCut;    //��ų �ƾ� ����
    public bool isEffect;    //��ų �ٷ� ���� ����
    public bool isUltimate;  //�ñر� ����
    public bool isSplash;    //���÷��� ����
    public bool isPenetrate; //���� ����
    
    public SkillEffect skillEffect;       //��ų ȿ��
    public SkillTarget skillTarget;       //��ų �߻� Ÿ��
    public ProjectileType projectileType; //��ų ���ư��� ����
}
