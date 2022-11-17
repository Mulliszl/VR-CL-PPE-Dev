
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using System.IO;
using UnityEngine;

namespace HelloWorld
{
    public class HelloWorldPlayerbis : NetworkBehaviour
    {
        public NetworkVariableVector3 Position = new NetworkVariableVector3(new NetworkVariableSettings
        {
            WritePermission = NetworkVariablePermission.ServerOnly,
            ReadPermission = NetworkVariablePermission.Everyone
        });

        public NetworkVariableQuaternion Rotation = new NetworkVariableQuaternion(new NetworkVariableSettings
        {
            WritePermission = NetworkVariablePermission.ServerOnly,
            ReadPermission = NetworkVariablePermission.Everyone
        });

        public override void NetworkStart()
        {
            Move();
        }


        public void Move()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                var PlayerPosition = GameObject.Find("Camera VR").transform.position;
                var PlayerRotation = GameObject.Find("Camera VR").transform.rotation;
                transform.position = PlayerPosition;
                transform.rotation = PlayerRotation;
                Position.Value = PlayerPosition;
                Rotation.Value = PlayerRotation;
            }
            else
            {
                SubmitPositionRequestServerRpc();
            }
        }

        [ServerRpc]
        void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            Position.Value = GetRandomPositionOnPlane();
        }

        static Vector3 GetRandomPositionOnPlane()
        {
            return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
        }

        void Update()
        {
            transform.position = Position.Value;
            Move();
        }
    }
}