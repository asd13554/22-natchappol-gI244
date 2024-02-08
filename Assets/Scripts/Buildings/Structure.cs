using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    [SerializeField]
    protected string structureName;
    public string StructureName { get { return structureName; } }

    [SerializeField] protected Sprite structurePic;
    public Sprite StructurePic { get { return structurePic; } }

    [SerializeField]
    protected int curHP;
    public int CurHP { get { return curHP; } set { curHP = value; } }

    [SerializeField]
    protected int maxHP;
    public int MaxHP { get { return maxHP; } set {  maxHP = value; } }

    [SerializeField]
    protected Faction faction;
    
    [SerializeField]
    protected GameObject selectionVisual;
    public GameObject SelectionVisual { get { return selectionVisual; } }

    void Start()
    {
        
    }// Start is called before the first frame update

    
    void Update()
    {
        
    }// Update is called once per frame
}
