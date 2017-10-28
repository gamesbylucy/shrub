using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Complex
{
    /**
     * @Brief Represents a complex of nodes.
     */
    /****************************************************************************************************
    * Public Members
    ****************************************************************************************************/
    public string name;
    public List<Node> members;
    public float rColor;
    public float gColor;
    public float bColor;
    public Color color = new Color();
    public bool isAbsorbed;
    public int size;
    public int complexity;

    /****************************************************************************************************
    * Private Members
    ****************************************************************************************************/

    /****************************************************************************************************
    * Static Members
    ****************************************************************************************************/

    /****************************************************************************************************
    * Constructor
    ****************************************************************************************************/
    public Complex()
    {
        initializeComplex();
    }

    /****************************************************************************************************
    * Public Methods
    ****************************************************************************************************/
    public void initializeComplex()
    {
        members = new List<Node>();
        size = 0;
        complexity = 0;
        setColor();
        setName();
    }

    public void add(Node theNode)
    {
        members.Add(theNode);
    }
    public void remove(Node theNode)
    {
        members.Remove(theNode);
    }
    public void clear()
    {
        members.Clear();
    }

    public void setColor()
    {
        rColor = (float)ShrubUtils.random.NextDouble();
        bColor = (float)ShrubUtils.random.NextDouble();
        gColor = (float)ShrubUtils.random.NextDouble();
        color = new Color(rColor, bColor, gColor);
    }

    public void setName()
    {
        name = "Complex " + rColor + gColor + bColor;
    }

    public void setMemberColor()
    {
        foreach (Node member in members)
        {
            member.setDecalColor(color);
        }
    }

    /****************************************************************************************************
    * Private Methods
    ****************************************************************************************************/
}
