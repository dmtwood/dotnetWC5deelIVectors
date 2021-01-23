using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace VectorLib
{
    public class Vector<T> : IEnumerable<T>
    {
        private int itemCount;
        private T[] items;

        /// <summary>
        /// Maakt lege vector aan met gegeven <paramref name="capaciteit"/>.
        /// </summary>
        /// <param name="capaciteit">De capaciteit van de vector bij aanmaak (standaard 1)</param>
        public Vector(int capaciteit = 1)
        {
            itemCount = 0;
            items = new T[capaciteit];
        }

        /// <summary>
        /// Uitbreiding 1: Initialiseert nieuwe Vector met dezelfde inhoud en grootte als gegeven collectie.
        /// </summary>
        /// <param name="collection">Collectie van het type <see cref="IEnumerable{T}"/> die zal gebruikt worden om de initiele waardes van de Vector op to vullen.</param>
        public Vector(IEnumerable<T> collection)
        {
            itemCount = collection.Count();
            items = collection.ToArray();
        }

        /// <summary>
        /// Voegt item toe achteraan de Vector.
        /// </summary>
        /// <param name="item">Toe te voegen item.</param>
        public void PushBack(T item)
        {
            if (itemCount == items.Length)
            {
                Grow();
            }

            items[itemCount] = item;
            itemCount++;
        }

        /// <summary>
        /// Verwijderd laatst toegevoegde item uit de vector.
        /// </summary>
        public void PopBack()
        {
            if (itemCount > 0)
                itemCount--;
        }

        /// <summary>
        /// Uitbreiding 2: om de collection initialiser te doen werken (new Vector { }).
        /// Zie: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/object-and-collection-initializers#collection-initializers
        /// </summary>
        /// <param name="item">Toe te voegen item.</param>
        public void Add(T item)
        {
            PushBack(item);
        }

        /// <summary>
        /// Geeft item terug op bepaalde index.
        /// </summary>
        /// <param name="index">Index van het gewenste item.</param>
        /// <returns>Het item op plaats <paramref name="index"/></returns>
        /// <exception cref="ArgumentOutOfRangeException">Als <paramref name="index"/> het bereik van de Vector overschrijdt.</exception>
        public T GetItem(int index)
        {
            if (index < itemCount && index >= 0)
            {
                return items[index];
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
        }

        /// <summary>
        /// Verdubbelt de capaciteit van de interne array.
        /// </summary>
        private void Grow()
        {
            // Array voorziet zelf een methode om capaciteit te veranderen
            Array.Resize(ref items, items.Length * 2);

            // Of, als je het manueel wilt doen:
            //T[] tmp = new T[items.Length * 2];
            //for (int i = 0; i < itemCount; i++)
            //{
            //    tmp[i] = items[i];
            //}
            //items = tmp;
        }

        /// <summary>
        /// Laat alle elementen van de Vector zien.
        /// </summary>
        /// <returns>Een met komma's gescheiden lijst van waarden in de Vector.</returns>
        public override string ToString()
        {
            return string.Join(", ", items.Take(itemCount));
        }

        public IEnumerator<T> GetEnumerator()
        {
            return VectorEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return VectorEnumerator();
        }

        private IEnumerator<T> VectorEnumerator()
        {
            for (int i = 0; i < itemCount; i++)
            {
                yield return items[i];
            }
        }
    }
}
