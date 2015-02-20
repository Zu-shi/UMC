#!/bin/bash
FILES=$(find . -name "*.cs")
for f in $FILES
do
  echo "Processing $f file..."
	cat $f | pbcopy && echo "#if UNITY_IPHONE
#elif UNITY_ANDROID
#else
" > $f && pbpaste >> $f
	echo "
#endif" >> $f
done