using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Essentials", menuName = "ScriptableObjects/PrefabSO", order = 1)]
public class PrefabSO : ScriptableObject
{
    public GameObject prefab; // Đối tượng prefab
    public bool destroyOld; // Lựa chọn xóa prefab cũ hay không

    // Phương thức để thay thế prefab
    public void ReplacePrefab(GameObject newPrefab)
    {
        // Tìm tất cả instance của prefab cũ trên scene và thay thế chúng bằng prefab mới
        GameObject[] objectsToReplace = GameObject.FindGameObjectsWithTag(prefab.tag);

        foreach (GameObject obj in objectsToReplace)
        {
            // Tạo prefab mới và đặt tên giống với prefab cũ
            GameObject newObject = Instantiate(newPrefab, obj.transform.position, obj.transform.rotation);
            newObject.name = obj.name;

            // Tự động xóa prefab cũ nếu được lựa chọn
            if (destroyOld)
            {
                DestroyImmediate(obj);
            }
            else
            {
                obj.SetActive(false);
            }
        }

        // Cập nhật prefab mới
        prefab = newPrefab;
    }
}