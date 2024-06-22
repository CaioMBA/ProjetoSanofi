import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter/painting.dart';
import 'package:sanofi_app/Domain/Settings/GlobalSchematics.dart';

class LoginPage extends StatefulWidget {
  const LoginPage({super.key});

  @override
  State<LoginPage> createState() => _LoginPageState();
}

class _LoginPageState extends State<LoginPage> {
  @override
  Widget build(BuildContext context) {
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
                mainAxisAlignment:  MainAxisAlignment.spaceEvenly,
              children: [
                Container(
                  width: MediaQuery.of(context).size.width * 0.3,
                  height: MediaQuery.of(context).size.height * 0.6,
                  margin: const EdgeInsets.only(bottom: 20),
                  padding: const EdgeInsets.symmetric(
                    horizontal: 30,
                    vertical: 5,
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
                      Image.asset('Images/Sanofi_Logo_Nome.png', width: MediaQuery.of(context).size.width * 0.1),
                      SizedBox(height: MediaQuery.of(context).size.height * 0.008),
                      Text(
                        'Micro Managment System',
                        style: TextStyle(
                          fontSize: 14,
                          color: GlobalSchematics().primaryColor,
                        ),
                      ),
                      const Text(
                        textAlign: TextAlign.left,
                        'Login',
                        style: TextStyle(
                          fontSize: 25,
                          fontWeight: FontWeight.bold,
                        ),
                      ),
                    ],
                  ),
                ),
              ],
            ),
            Container(
              decoration: BoxDecoration(
                borderRadius: BorderRadius.circular(50)
              ),
              width: MediaQuery.of(context).size.width * 0.45,
              height: MediaQuery.of(context).size.height * 0.8,
              child: const Image(
                image: AssetImage('Images/MainImage.jpg'),
                fit: BoxFit.contain
              ),
            )
          ],
        ),
      ),
    );
  }
}

