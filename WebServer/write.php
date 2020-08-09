<?php
	if ($_SERVER['REQUEST_METHOD'] != 'POST')
	{
		echo 'invalid request method';
		return;
	}

	try
	{
		$token = fgets(fopen('../private/write_token', 'r'));
		$POST = json_decode(file_get_contents('php://input'), true);
	} catch (Exception $e) {
		echo "failure: ${e}";
		exit();
	}

	function put_new_map($machine_name, $map_code, $map_content, $sha)
	{
		global $token;

		$timestamp = (new DateTime())->format('Y-m-d H:i:s');

		$content = json_encode(array(
			'message' => "Last update in ${timestamp} by ${machine_name} - ${map_code}",
			'sha' => $sha,
			'content' => $map_content
		));

		$ch = curl_init('https://api.github.com/repos/SolarSystemDefense/gamedb/contents/ExportedMaps.json');
		curl_setopt($ch, CURLOPT_HTTPHEADER, array(
			'User-Agent: SSD WebServer',
			"Authorization: token ${token}",
			'Access: application/vnd.github.v3+json',
			'Content-Type: application/json'
		));
		curl_setopt($ch, CURLOPT_CUSTOMREQUEST, 'PUT');
		curl_setopt($ch, CURLOPT_POSTFIELDS, $content);
		curl_setopt($ch, CURLOPT_RETURNTRANSFER, 1);
		curl_exec($ch);
		$http_status = curl_getinfo($ch, CURLINFO_HTTP_CODE);
		curl_close($ch);

		return $http_status;
	}

	function get_last_exported_map()
	{
		$maps = json_decode(file_get_contents('https://raw.githubusercontent.com/SolarSystemDefense/gamedb/master/ExportedMaps.json'), true);
		return max(array_column($maps, 'Code'));
	}

	function get_sha(&$map_content)
	{
		$map_content = base64_decode($map_content);
		try
		{
			$map_content = json_decode($map_content, true);
			if (!($map_content["Code"] && $map_content["Walkpoints"])){}
		} catch(Exception $e)
		{
			echo "invalid map content: ${e}";
			exit();
		}

		$content = json_decode(file_get_contents('https://api.github.com/repos/SolarSystemDefense/gamedb/contents/ExportedMaps.json', false, stream_context_create([
			'http' => [
				'method' => 'GET',
				'header' => 'User-Agent: SSD WebServer'
			]
		])));

		$maps = json_decode(base64_decode($content -> content), true);
		array_push($maps, $map_content);
		$map_content = base64_encode(json_encode($maps));

		return $content -> sha;
	}

	function get_post_var($x)
	{
		global $POST;
		if (!isset($POST[$x]))
		{
			echo "missing parameter ${x}.";
			exit();
		}

		return $POST[$x];
	}

	function validate_content()
	{
		$machine_name = get_post_var('machine_name');
		$map_code = get_post_var('map_code');
		$map_content = get_post_var('map_content');

		if ($map_code[0] != '@' || (int)substr($map_code, 1) != get_last_exported_map() + 1)
		{
			echo "invalid map code. ${map_code}";
			return;
		}

		try {
			$sha = get_sha($map_content);
			echo put_new_map($machine_name, $map_code, $map_content, $sha);
		} catch (Exception $e) {
			echo "failure: ${e}";
		}
	}
	validate_content();
?>