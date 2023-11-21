using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeweyLibrary
{
    /// <summary>
    /// enum for setting the colour of the node
    /// </summary>
    public enum Color
    {
        Red,
        Black
    }

    /// <summary>
    /// Red Black Tree for dewey decimal system
    /// code adapted from code by Rudolf Holzhausen
    /// </summary>
    public class RBDeweyTree
    {
        #region Constructor
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// class of node
        /// </summary>
        public class Node
        {
            //colour of node
            public Color colour;

            //node links
            public Node left;
            public Node right;
            public Node parent;

            //dewey decimal entry
            public DeweyDecimalClass DeweyCat;

            //get/set for node attributes
            public Node(DeweyDecimalClass deweyCat) { this.DeweyCat = deweyCat; ; }
            public Node(Color colour) { this.colour = colour; }
            public Node(Color colour, DeweyDecimalClass deweyCat)
            {
                this.DeweyCat = deweyCat;
                this.colour = colour;
            }
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Root node of the tree (both reference & pointer)
        /// </summary>
        public Node root;
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Constructor - New instance of a Red-Black tree object
        /// </summary>
        public RBDeweyTree()
        {
        }
        //---------------------------------------------------------------------------------------//
        #endregion


        #region Rotation Methods
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Left Rotate
        /// </summary>
        /// <param name="X"></param>
        /// <returns>void</returns>
        private void LeftRotate(Node X)
        {
            if (X == null || X.right == null) return; // Add null checks at the beginning

            Node Y = X.right; // set Y
            Node XParent = X.parent; // store X's parent

            // turn Y's left subtree into X's right subtree
            X.right = Y.left;
            if (Y.left != null)
            {
                Y.left.parent = X;
            }

            // link X's parent to Y
            Y.parent = XParent;
            if (XParent == null)
            {
                root = Y;
            }
            else if (X == XParent.left)
            {
                XParent.left = Y;
            }
            else
            {
                XParent.right = Y;
            }

            // put X on Y's left
            Y.left = X;
            X.parent = Y;
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Rotate Right
        /// </summary>
        /// <param name="Y"></param>
        /// <returns>void</returns>
        private void RightRotate(Node Y)
        {
            if (Y == null || Y.left == null) return; // Add null checks at the beginning

            Node X = Y.left; // set X
            Node YParent = Y.parent; // store Y's parent

            // turn X's right subtree into Y's left subtree
            Y.left = X.right;
            if (X.right != null)
            {
                X.right.parent = Y;
            }

            // link Y's parent to X
            X.parent = YParent;
            if (YParent == null)
            {
                root = X;
            }
            else if (Y == YParent.right)
            {
                YParent.right = X;
            }
            else
            {
                YParent.left = X;
            }

            // put Y on X's right
            X.right = Y;
            Y.parent = X;
        }
        //---------------------------------------------------------------------------------------//
        #endregion


        #region Insertion
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// Insert a new object into the RB Tree
        /// </summary>
        /// <param name="deweyCat"></param>
        public void Insert(DeweyDecimalClass deweyCat)
        {
            //new item node
            Node newItem = new Node(deweyCat);
            newItem.colour = Color.Red; // colour the new node red
            newItem.left = null;
            newItem.right = null;

            if (root == null)
            {
                root = newItem;
                root.colour = Color.Black;
                return;
            }

            Node Y = null;
            Node X = root;
            while (X != null) // sort by call number
            {
                Y = X;
                X = newItem.DeweyCat.Number < X.DeweyCat.Number ? X.left : X.right;
            }

            newItem.parent = Y;
            if (newItem.DeweyCat.Number < Y.DeweyCat.Number) // sort by call number
            {
                Y.left = newItem;
            }
            else
            {
                Y.right = newItem;
            }
            //call method to check for violations and fix
            InsertFixUp(newItem); 
        }
        //---------------------------------------------------------------------------------------//
        /// <summary>
        /// fix insert
        /// </summary>
        /// <param name="item"></param>
        private void InsertFixUp(Node item)
        {
            //Checks Red-Black Tree properties
            while (item != root && item.parent.colour == Color.Red)
            {
                //violation found
                if (item.parent == item.parent.parent.left)
                {
                    Node Y = item.parent.parent.right;
                    if (Y != null && Y.colour == Color.Red) //Case 1: uncle is red
                    {
                        item.parent.colour = Color.Black;
                        Y.colour = Color.Black;
                        item.parent.parent.colour = Color.Red;
                        item = item.parent.parent;
                    }
                    else //Case 2: uncle is black
                    {
                        if (item == item.parent.right)
                        {
                            item = item.parent;
                            LeftRotate(item);
                        }
                        // Case 3: recolour & rotate
                        item.parent.colour = Color.Black;
                        item.parent.parent.colour = Color.Red;
                        RightRotate(item.parent.parent);
                    }
                }
                else
                {
                    // Mirror image of code above
                    Node X = item.parent.parent.left;
                    if (X != null && X.colour == Color.Red) //Case 1
                    {
                        item.parent.colour = Color.Black;
                        X.colour = Color.Black;
                        item.parent.parent.colour = Color.Red;
                        item = item.parent.parent;
                    }
                    else //Case 2
                    {
                        if (item == item.parent.left)
                        {
                            item = item.parent;
                            RightRotate(item);
                        }
                        //Case 3: recolour & rotate
                        item.parent.colour = Color.Black;
                        item.parent.parent.colour = Color.Red;
                        LeftRotate(item.parent.parent);
                    }
                }
            }
            root.colour = Color.Black; //Re-colour the root black as necessary
        }
        //---------------------------------------------------------------------------------------//
        #endregion


        #region Search
        //---------------------------------------------------------------------------------//
        /// <summary>
        /// Find item in the tree ()
        /// </summary>
        /// <param name="key"></param>
        public DeweyDecimalClass FindByCallNumber(int callnumber)
        {
            Node temp = root;
            while (temp != null)
            {
                if (callnumber < temp.DeweyCat.Number) //go left
                {
                    temp = temp.left;
                }
                else if (callnumber > temp.DeweyCat.Number) //go right
                {
                    temp = temp.right;
                }
                else
                {
                    //dewey entry is found in tree
                    DeweyDecimalClass decimalClass = new DeweyDecimalClass();
                    decimalClass.Number = temp.DeweyCat.Number;
                    decimalClass.Description = temp.DeweyCat.Description;
                    decimalClass.Level = temp.DeweyCat.Level;
                    return decimalClass;
                }
            }
            //dewey entry not found
            return null;
        }
        //---------------------------------------------------------------------------------------//
    

        #endregion
    }
}
//-----------------------------------------------oO END OF FILE Oo----------------------------------------------------------------------//