import connection from "../api/signalRConnection";

export const joinGroup = async (groupName: string) => {
  try {
    await connection.invoke("JoinTaskGroup", groupName);
    console.log(`✅ Joined group: ${groupName}`);
  } catch (err) {
    console.error(`❌ Error joining group ${groupName}:`, err);
  }
};

export const leaveGroup = async (groupName: string) => {
  try {
    await connection.invoke("LeaveTaskGroup", groupName);
    console.log(`✅ Left group: ${groupName}`);
  } catch (err) {
    console.error(`❌ Error leaving group ${groupName}:`, err);
  }
};
