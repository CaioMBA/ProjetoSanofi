import 'package:universal_html/html.dart';

class LocalStorageHelper{
  static Storage localStorage = window.localStorage;

  static void saveData(Map<String, dynamic> data){
    for (String key in data.keys){
      localStorage[key] = data[key];
    }
  }
  static dynamic getData(String key){
    return localStorage[key];
  }
  
  static void removeData(String key){
    localStorage.remove(key);
  }

  static void clearAllData(){
    localStorage.clear();
  }
}

class SessionStorageHelper{
  static Storage sessionStorage = window.sessionStorage;

  static void saveData(Map<String, dynamic> data){
    for (String key in data.keys){
      sessionStorage[key] = data[key];
    }
  }
  static dynamic getData(String key){
    return sessionStorage[key];
  }

  static void removeData(String key){
    sessionStorage.remove(key);
  }

  static void clearAllData(){
    sessionStorage.clear();
  }
}