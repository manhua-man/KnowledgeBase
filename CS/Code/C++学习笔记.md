## **c基础理解**

### 数据类型存在的意义

给变量分配合适的内存空间

ASCII码由两部分组成
ASCII非打印控制字符：0-31分配给了控制字符如换行等
ASCII打印字符32-126分配给了能在键盘上找到的字符

++a前置递增会让变量+1然后进行表达式运算
a++后置递增先进行表达式运算，后让变量+1

### continue语句

在循环语句中，跳过本次循环中余下尚未执行的语句，继续下一次循环

### goto语句

无条件跳转代码

```c++
count<< "1、xxxx" <<endl;
goto Flag;
count<< "2、xxxx" <<endl;
count<< "3、xxxx" <<endl;
Flag;
count<< "4、xxxx" <<endl;
```

指针在32位操作系统下占四个字节空间大小，不管是什么数据类型
指针在64位操作系统下占八个字节空间大小，不管是什么数据类型

空指针内存无法访问
野指针指向非法内存空间
const指针指针指向可以修改，指针指向的值无法修改 


c++中没有一个明先的生命周期，自己要注意方法的调用时机

## c++核心编程

### 内存分区

- 代码区；存放函数体的二进制编码，操作系统管理
- 全局区；存放全局变量和静态变量以及常量
- 栈区;由编译器自动分配释放，存放函数的参数值，局部变量等
- 堆区；由程序员自行分配释放，或程序结束操作系统回收

### 内存四区的意义

不同区域存放的数据，赋予不同的生命周期，编程更加灵活  



### 引用：

数据类型 &别名 =原名

```c++
int a=10;
int &b=a;
cout<<"a="<<a<<endl;
cout<<"b="<<b<<endl;

b=100;
cout<<"a="<<a<<endl;
cout<<"b="<<b<<endl;
```

注意事项：引用要初始化，初始化后不可以更改

### 重载：

不要在参数中赋值，容易出现二义性

封装：

struct和class的区别

struct默认权限是公共public

class默认权限是私有private

### 对象特性：

#### 构造函数

没有返回值 不用写void

函数名与类名相同

构造函数可以有参数，可以发生重载

创建对象的时候，构造函数会自动调用

会自动调用，只会调用一次

```c++
class Person
{
public:
      Person()
      {
          cout<<"Person构造函数调用"<<endl;
      }
}
```

#### 析构函数

没有返回值 不用写void

函数名与类名相同，在名称前加~

不可以有参数

会自动调用，只会调用一次

```c++
public:
      ~Person()
      {
          cout<<"Person析构函数调用"<<endl;
      }
}
```

我们不提供构造和析构函数的话，编译器会自己提供空实现的构造和析构

#### 构造函数的分类

有参和无参

普通构造和拷贝构造

```c++
class Person
{
public:
      Person(int a)
      {
          age=a;
          cout<<"Person构造函数调用"<<endl;
      }
}

拷贝函数
public:
      Person(const Person &p)
      {
          age=p.age;
          cout<<"Person构造函数调用"<<endl;
      }
}
}
//调用
void test01（）
{
    //1.括号法
    Person p1;//默认构造函数调用
    Person p2(10)//有参构造
    Person p3(p2)//拷贝构造
    //2.显示法
     Person p1;//默认构造函数调用
     Person p2=Person(10)//有参构造
     Person p3=Person(p3)//有参构造
    //3.隐式转换法
    Person p2=10;//Person p2=Person(10);
    Person p3=p2;//拷贝构造
}
```

#### 深拷贝与浅拷贝（面试常问）

浅拷贝：简单的赋值拷贝操作

深拷贝：在堆区重新申请空间，进行拷贝操作

浅拷贝带来的问题就是堆区的内存重复释放,如果属性有在堆区开辟的，一定要自己提供拷贝构造函数

```c++
class Person
{
Public:
    Person()
    {
        cout<<"Person的默认构造函数调用"<<endl;
    }
    Person(int age,int height)
    {
        m_Age=age;
        cout<<"Person的有参函数的调用"<<endl;
    }
    //拷贝构造函数
    Person(const Person& p)
    {
        cout<<"拷贝构造函数"<<endl;
        //如果不利用深拷贝在堆区创建新内存，会导致浅拷贝带来的重复释放堆区问题
        m_age=p.m_age;
        m_height=new int(*p.m_height);
    }
   ~Person()
   {
       //西沟代码，将堆区开辟数据做释放操作
       if(m_Height)
       {
           delete m_Height;
           m_Height=NULL;
       }
       cout<<"Person的析构函数调用"<<endl;
   }
 public:
    int m_Age;
    int*m_Height;
}

void test01()
{
    Person p1(18,160);
    cout<<"p2的年龄为："<<p1.m_Age<<"身高为："<<*p1.m_Height<<endl;
    Person p2(p1);
    cout<<"p2的年龄为："<<p2.m_Age<<"身高为："<<*p2.m_Height<<endl;
}
```

#### 初始化列表

```c++
//传统方式初始化
//Person(inta, intb,intc)
//{
//m_A=a;
//m_B=b;
//m_C=c;
//}

//初始化列表方式初始化
Person(int a,int b,int c):m_A(a),m_B(b),m_C(c){}
void PrintPerson(){
    cout<<"mA:"<<m_A<<endl;
    cout<<"mB:"<<m_B<<endl;
    cout<<"mC:"<<m_C<<endl;
}
```

成员函数调用时前面默认带this



#### 友元

友元关键字为friend

```
class Builiding{

//goodGay全局函数是Building好朋友，可以访问Building中的私有变量
friend void goodGay(Building *building)
public:
 {
 Building()
 {
 
  m_SittingRoom="客厅";
  m_BedRoom="卧室";
  }
  public:
  string m_SittingRoom;
  private:
  string m_BedRoom;
 }
 
 void goodGay(Building *building)
 {
 
 }
}
```

#### 运算符重载

```c++
class Person
{
    public:
    //1、成员函数重载+号
    Person operator+(Person &p)
    {
        Person temp;
        temp.m_A=this->m_A+p.m_A;
        temp.m_B=this_>m_B+p.m_B;
        return temp;
    }
    //2、全局函数重载+号
    Person operator+(Person &p1,Person &p2)
    {
        Person temp;
        temp.m_A=p1.m_A+p2.m_A;
        temp.m_B=p1.m_A+p2.m_B;
        return temp;
    }
      int m_A;
      int m_B;
};
void test01()
{
    Person p1;
    p1.m_A=10;
    p1.m_B=10;
    Person p2;
    p2.m_A=10;
    p2.m_B=10;
    
    Person p3=p1+p2;
    //成员函数重载本质调用
    Person p3=p1.operator+(p2);
    //全局函数重载本质调用
    Person p3=operator(p1,p2);
    //运算符重载，也可以发生函数重载
}
```

## C++提高编程

### 模板

```c++
template <typename T> //声明一个模板

```



## c++编码过程中注意事项

同一个源文件下要先执行的方法在代码中的位置也要在前面

## C++常用API

### System

```c++
system("pause")
system("cls")
```



### Stream

```
ofstream//在本地保存数据
ofs.open(FILENAME, ios::out);打开
ofs.close();关闭

ifStream//在本地读取数据
```





























































































































































































