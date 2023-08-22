using ModestTree;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Gameloops.Save
{
    [DefaultExecutionOrder(-100)]
    public class EntitySaver<T> : MonoBehaviour
    {
        [SerializeField] private string key = "";
        private ISaveableEntity<T> _saveableEntity;
        private bool _saveQueued = false;
        private float _currentTimer = 0;
        private const float TimeBetweenSaves = 1f;

        private void OnValidate()
        {
            if (key.IsEmpty()) key = gameObject.name + "_" + Random.Range(0, 100);
        }

        private void Awake()
        {
            _saveableEntity = GetComponent<ISaveableEntity<T>>();
        }

        private void Start()
        {
            Load();
        }

        private void Update()
        {
            _currentTimer -= Time.deltaTime;
            if(_currentTimer <= 0f && _saveQueued) Save();
        }

        private void OnEnable()
        {
            _saveableEntity.OnSave += Save;
        }

        private void OnDisable()
        {
            _saveableEntity.OnSave -= Save;
        }

        private void Save()
        {
            if (_currentTimer > 0)
            {
                _saveQueued = true;
                return;
            }

            ES3.Save(key, ES3.KeyExists(key) ? _saveableEntity.GetEntity() : _saveableEntity.GetEntityDefault());
            _currentTimer = TimeBetweenSaves;
        }

        private void Load()
        {
            if(ES3.KeyExists(key)) _saveableEntity.SetEntity(ES3.Load<T>(key));
            else Save();
        }

        public void Delete()
        {
            if(ES3.KeyExists(key)) ES3.DeleteKey(key);
        }
    }
}