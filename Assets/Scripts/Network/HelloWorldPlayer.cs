
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UnityEngine;
using UnityEngine.SpatialTracking;

// allow player movement in multiplayer VR scenes

namespace HelloWorld
{
    
    public class HelloWorldPlayer : NetworkBehaviour
    {
        public TrackedPoseDriver myTPD;
        Transform cameraTransform;
        private void Start()
        {
            myTPD = GetComponent<TrackedPoseDriver>();
            cameraTransform = GetComponentInChildren<Camera>().transform;

        }


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
                //      ** old **
                //var randomPosition = GetRandomPositionOnPlane();
                //transform.position = randomPosition;
                //Position.Value = randomPosition;

                //      ** get position from main camera **
                //var PlayerPosition = GameObject.Find("Camera VR").transform.position;
                //var PlayerRotation = GameObject.Find("Camera VR").transform.rotation;
                //transform.position = PlayerPosition;
                //transform.rotation = PlayerRotation;
                //Position.Value = PlayerPosition;
                //Rotation.Value = PlayerRotation;
            }
            else
            {
                SubmitPositionRequestServerRpc();
            }
        }

        [ServerRpc]
        void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            
        }



        void Update()
        {
            if (IsLocalPlayer)
            {
                Move();
            }

        }
            //transform.position = Position.Value;
            
        }
    }
