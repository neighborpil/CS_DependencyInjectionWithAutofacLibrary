# CS_DependencyInjectionWithAutofacLibrary
Sample codes following the udemy course

Dependancy Injection
=========================================
#의미
 - 기존의 생성자를 통하여 클래스를 생성하지 dependency를 아는 container를 통하여
   클래스를 생성 및 관리하는 방법
* 장점
 - boilerplate code를 줄일 수 있다
 - 한 곳에서 연관된 클래스를 컨트롤 가능하다
 - 기능상 요구가 있을 때 다시 컴파일 하지 않고, Json, Xml등으로 일부만 바꾸어
   새로운 기능을 실행 시킬 수있다
 - 클래스의 lifetime control이 쉽다
 - Encapsulation
 
* 단점
 - 익숙하지 않은 사람들과 헥갈리기 쉽다
 - 생성자를 보는 것이 아니라, 클래스, 인터페이스 관계를 봐야 하기 때문에 안 익숙하다


Inversion of Control(IoC)
=========================================
# 의미
 - Generic을 이용하여 영어의 어순과 비슷하게 메소드를 재정립하여 가독성을 높인다
