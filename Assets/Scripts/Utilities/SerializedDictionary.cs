#if UNITY_2020_1_OR_NEWER
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public sealed class SerializedDictionary<TK, TV> : IDictionary<TK, TV>, ISerializationCallbackReceiver
{
    [Serializable]
    private struct KeyValuePair
    {
        [SerializeField]
        private TK key;
        [SerializeField]
        private TV value;

        public KeyValuePair(TK key, TV value)
        {
            this.key = key;
            this.value = value;
        }

        public TK Key
        {
            get => key;
            set => key = value;
        }

        public TV Value
        {
            get => value;
            set => this.value = value;
        }
    }
    
    [SerializeField]
    private List<KeyValuePair> pairs = new();
    private readonly Dictionary<TK, int> _indexByKey = new();
    private readonly Dictionary<TK, TV> _dictionary = new();
    
    private void UpdateIndexes(int removedIndex)
    {
        for (var i = removedIndex; i < pairs.Count; i++)
        {
            var key = pairs[i].Key;
            _indexByKey[key]--;
        }
    }
    
    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        // Method intentionally left empty.
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        _dictionary.Clear();
        _indexByKey.Clear();

        for (var i = 0; i < pairs.Count; i++)
        {
            var key = pairs[i].Key;
            if (key != null && !ContainsKey(key))
            {
                _dictionary.Add(key, pairs[i].Value);
                _indexByKey.Add(key, i);
            }
        }
    }

    public void Add(TK key, TV value)
    {
        if(!pairs.Contains(new KeyValuePair(key,value))) pairs.Add(new KeyValuePair(key, value));
        _dictionary.TryAdd(key, value);
        _indexByKey.TryAdd(key, pairs.Count - 1);
    }

    public bool ContainsKey(TK key)
    {
        return _dictionary.ContainsKey(key);
    }

    public bool Remove(TK key)
    {
        if (_dictionary.Remove(key))
        {
            var index = _indexByKey[key];
            pairs.RemoveAt(index);
            UpdateIndexes(index);
            _indexByKey.Remove(key);
            return true;
        }
        
        return false;
    }

    public bool TryGetValue(TK key, out TV value)
    {
        return _dictionary.TryGetValue(key, out value);
    }

    public void Clear()
    {
        pairs.Clear();
        _dictionary.Clear();
        _indexByKey.Clear();
    }

    [Obsolete("Use BuildNativeDictionary instead.")]
    public Dictionary<TK, TV> BuiltNativeDictionary()
    {
        return new Dictionary<TK, TV>(_dictionary);
    }
    
    public Dictionary<TK, TV> BuildNativeDictionary()
    {
        return new Dictionary<TK, TV>(_dictionary);
    }
    
    void ICollection<KeyValuePair<TK, TV>>.Add(KeyValuePair<TK, TV> pair)
    {
        Add(pair.Key, pair.Value);
    }
    
    bool ICollection<KeyValuePair<TK, TV>>.Contains(KeyValuePair<TK, TV> pair)
    {
        if (_dictionary.TryGetValue(pair.Key, out var value))
        {
            return EqualityComparer<TV>.Default.Equals(value, pair.Value);
        }
        else
        {
            return false;
        }
    }
    
    bool ICollection<KeyValuePair<TK, TV>>.Remove(KeyValuePair<TK, TV> pair)
    {
        if (_dictionary.TryGetValue(pair.Key, out var value))
        {
            var isEqual = EqualityComparer<TV>.Default.Equals(value, pair.Value);
            if (isEqual)
            {
                return Remove(pair.Key);
            }
        }

        return false;
    }
    
    void ICollection<KeyValuePair<TK, TV>>.CopyTo(KeyValuePair<TK, TV>[] array, int arrayIndex)
    {
        ICollection collection = _dictionary;
        collection.CopyTo(array, arrayIndex);
    }
    
    IEnumerator<KeyValuePair<TK, TV>> IEnumerable<KeyValuePair<TK, TV>>.GetEnumerator()
    {
        return _dictionary.GetEnumerator();
    }
    
    IEnumerator IEnumerable.GetEnumerator()
    {
        return _dictionary.GetEnumerator();
    }
    
    public int Count => _dictionary.Count;
    
    public bool IsReadOnly => false;
    
    public ICollection<TK> Keys => _dictionary.Keys;
    
    public ICollection<TV> Values => _dictionary.Values;
    
    public TV this[TK key]
    {
        get => _dictionary[key];
        set
        {
            _dictionary[key] = value;
            if (_indexByKey.TryGetValue(key, out var index))
            {
                pairs[index] = new KeyValuePair(key, value);
            }
            else
            {
                pairs.Add(new KeyValuePair(key, value));
                _indexByKey.Add(key, pairs.Count - 1);
            }
        }
    }
}
#endif