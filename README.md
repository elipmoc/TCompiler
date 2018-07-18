# 使い方

まず、これは、TGUIとTcompilerの二種類のプロジェクトから成り立っています。

まず、コアである、TCompilerから説明します。

TCompilerは最新コンパイラ構成技法という本の最初らへんに乗ってある簡易プログラミング言語から発想を得た、

新しいプログラミング言語をコンパイルするためのコンパイラです。

まず、TCompilerプロジェクトのbin/\Debugにtc.exeがあるので、ここまでコマンドプロンプトを使ってパス移動しておきます。

tc -v

と打つと、バージョン情報表示され。

tc -h

と打つと使い方が表示されます。

試しに、tc.exeと同じディレクトリに

source.txtというファイルを作って、その中に

print(1+5)

と書き保存します。

そして

tc -o source.txt aaa

と打つとビルドされ、aaa.exeが生成されます。

これはダブルクリックで普通に実行することができます。

次にTGUIについて説明します

TGUIはTCompilerをGUIで扱えるようにするためのプログラムです。

TGUIプロジェクトのbin/\DebugにTGUI.exeがあるのでcmdではなくダブルクリックで実行してください。

実行したら、ファイルメニューから新規プロジェクト作成してください。

そのあとテキストボックスにprint(1+5)と書いて、実行またはビルドを押すとexeが生成されます。

次にTCompilerが扱う言語仕様について説明します。

## 【型】

使える型はint型だけです。

## 【演算子】

+,-,*,/,<<,>>==,=,<,>,<=,>=,!=,%,|,||,&,&&,^などの一般的な二項演算子が使用できます。

## 【変数】

a=4+5

のように変数を使えます。

変数の作成は型宣言する必要はなく、単純に　a=式　で生成されます。

変数名は小文字のaからzまでしか使えません。

変数はデフォルトで0です

## 【if式】

if 式　then 式 else 式

でifが使えます、

0がfalseで

それ以外がtrueとして扱われます。

このifは式なので

print(if 4==4 then 111 else 222)

のように使えます。

## 【goto】

@ラベル名

でラベルが定義でき、

goto ラベル名

でジャンプできます。

ラベル名は小文字のaからzまでしか使えません。

例

```

goto p
print(7)
@p
/*何も表示されないプログラム*/

```

## 【関数】

関数は作成することができません。

しかし、print,scanという二つの関数が組み込みで用意しています。

print(式)

scan()

という風に使います

printは標準出力

scanは数字だけを受け付ける標準入力です

print(式)は式をそのまま返すので

print(print(1)+print(2))

という面白いことができます。


## 【カッコ】

()で優先順位を操作できます

(4+5)*3

## 【順次】
print(1)

print(2)print(3)

という風に複数の式を書くことができます。

上から順番に評価されます。

## 【コメントアウト】

/**/でコメントアウトができる

## 【複雑な例】

```

/*入力された数字までの和を計算します*/
max=scan()
i=1
sum=0

@loop
sum=sum+i
i=i+1
if i>max then goto after else goto loop
@after
print(sum)

```
