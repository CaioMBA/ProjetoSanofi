import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter/painting.dart';
import 'package:sanofi_app/Components/Widgets/CommonAlertBox.dart';
import 'package:sanofi_app/Domain/Settings/GlobalSchematics.dart';
import 'package:sanofi_app/Services/LoginServices.dart';

import '../Components/Widgets/CommonBorderRoundedButton.dart';
import '../Components/Widgets/CommonTextInput.dart';
import 'Home.dart';

class LoginPage extends StatefulWidget {
  const LoginPage({super.key});

  @override
  State<LoginPage> createState() => _LoginPageState();
}

class _LoginPageState extends State<LoginPage> {
  @override
  Widget build(BuildContext context) {
    final TextEditingController _userController = TextEditingController();
    final TextEditingController _passwordController = TextEditingController();

    submit() async {
      if (_userController.text.isEmpty || _passwordController.text.isEmpty){
        return showDialog(
          context: context,
          builder: (BuildContext context){
            return const CommonAlertBox(message: 'Preencha todos os campos', type: 'error',);
          }
        );
      }

      final response = await Login().login(_userController.text, _passwordController.text);
      if (response){
        Navigator.pushReplacement(
            context, MaterialPageRoute(builder: (context) => const HomePage()));
        return;
      }
      return showDialog(
          context: context,
          builder: (BuildContext context){
          return const CommonAlertBox(message: 'Credenciais inválidas', type: 'error',);
      });
    }



    return Scaffold(
      backgroundColor: GlobalSchematics().whiteColor,
      body: Container(
        alignment: Alignment.center,
        margin: const EdgeInsets.all(20),
        decoration:  BoxDecoration(
          borderRadius: BorderRadius.circular(30),
          image: const DecorationImage(
            image: AssetImage('/Images/Background_Image.jpg'),
            fit: BoxFit.cover
          )
        ),
        child: Row(
          mainAxisAlignment: MainAxisAlignment.spaceEvenly,
          children: [
            Column(
                mainAxisAlignment:  MainAxisAlignment.center,
              children: [
                Container(
                  width: MediaQuery.of(context).size.width * 0.25,
                  height: MediaQuery.of(context).size.height * 0.6,
                  margin: EdgeInsets.only(bottom: MediaQuery.of(context).size.height * 0.02),
                  padding: EdgeInsets.symmetric(
                    horizontal: MediaQuery.of(context).size.width * 0.03,
                    vertical: MediaQuery.of(context).size.height * 0.005,
                  ),
                  decoration: BoxDecoration(
                    borderRadius: BorderRadius.circular(20),
                    color: GlobalSchematics().whiteColor,
                    boxShadow: [
                      BoxShadow(
                        color: Colors.grey.withOpacity(0.5),
                        spreadRadius: 5,
                        blurRadius: 7,
                      )
                    ]
                  ),
                  child: Column(
                    children: [
                      Image.asset('Images/Sanofi_Logo_Nome.png', width: MediaQuery.of(context).size.width * 0.15),
                      Text(
                        'Micro Managment System',
                        style: TextStyle(
                          fontSize: 14,
                          color: GlobalSchematics().primaryColor,
                        ),
                      ),
                      SizedBox(height: MediaQuery.of(context).size.height * 0.05),
                      Row(
                        mainAxisAlignment: MainAxisAlignment.start,
                        children: [
                          SizedBox(width: MediaQuery.of(context).size.width * 0.005),
                          const Text(
                            'Login',
                            style: TextStyle(
                              fontSize: 20,
                              fontWeight: FontWeight.bold,
                            ),
                          ),],
                      ),
                      SizedBox(height: MediaQuery.of(context).size.height * 0.01),
                      CommonTextInput(
                        labelText: 'Usúario',
                        hintText: 'Digite seu Usuario',
                        inputType: 'TEXT',
                        controller: _userController,
                      ),
                      SizedBox(height: MediaQuery.of(context).size.height * 0.015),
                      CommonTextInput(
                        labelText: 'Senha',
                        hintText: 'Digite sua senha',
                        inputType: 'TEXT',
                        isPassword: true,
                        controller: _passwordController,
                      ),
                      SizedBox(height: MediaQuery.of(context).size.height * 0.02),
                      CommonBorderRoundedButton(
                        text: 'Entrar',
                        onTap: submit,
                      ),
                    ],
                  ),
                ),
              ],
            ),
            Container(
              decoration: BoxDecoration(
                borderRadius: BorderRadius.circular(20)
              ),
              height: MediaQuery.of(context).size.height * 0.98,
              child: SizedBox(
                height: MediaQuery.of(context).size.height * 0.9,
                width: MediaQuery.of(context).size.width * 0.35,
                child: const Image(
                  image: AssetImage('Images/MainImage.jpg'),
                  fit: BoxFit.fill,
                ),
              ),
            )
          ],
        ),
      ),
    );
  }
}

