<?php
for ($i = 0; strpos(md5('ckczppom' . $i), '000000') !== 0; $i++);
echo $i . PHP_EOL;
?>
