import 'package:sanofi_app/Data/API/DAO/AuthDAO.dart';
import '../Domain/AuthModels.dart';
import '../Domain/Settings/WebHelper.dart';

class Login{
  Future<bool> login(String user, String password) async {
    AuthTokenResponse response = await AuthDao().login(user, password);
    if (response.success){
      SessionStorageHelper.saveData({
        'Auth': response
      });
      return true;
    }
    return false;
  }
}