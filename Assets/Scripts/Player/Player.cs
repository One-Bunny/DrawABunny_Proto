using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OneBunny
{
    public class Player : FSMRunner<Player>, IFSMRunner
    {
        public enum States : int
        {
            Start,


            End
        }


        private void Awake()
        {

        }

        protected override void Update()
        {

        }



    }
}
