import cv2
import mediapipe as mp
import socket
import json

UDP_ID = "127.0.0.1"
UDP_PORT = 5055
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

mp_pose = mp.solutions.pose
pose = mp_pose.Pose()

cap = cv2.VideoCapture(0)

while cap.isOpened():
	ret, frame = cap.read()
	if not ret:
		break

	img_rgb = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
	results = pose.process(img_rgb)

	points = []

	if results.pose_landmarks:
		for lm in results.pose_landmarks.landmark:
			x = int(lm.x * frame.shape[1])
			y = int(lm.y * frame.shape[0])
			points.append([x, y])
			cv2.circle(frame, (x, y), 5, (0, 255, 0), -1)

	if points:
		data = json.dumps(points)
		sock.sendto(data.encode(), (UDP_ID, UDP_PORT))

	cv2.imshow("Cam feed", frame)
	if cv2.waitKey(1) & 0XFF == ord('q'):
		break

cap.release()
cv2.destroyAllWindows()