import TaskList from "./components/Task/TaskList";
import {
  BrowserRouter as Router,
  Routes,
  Route,
  Navigate,
} from "react-router-dom";
import Layout from "./components/Global/Layout";
import About from "./components/Global/About";
import TaskDetail from "./components/Task/TaskDetail";
import AddTask from "./components/Task/AddTask";
import UpdateTask from "./components/Task/UpdateTask";
import LandingPage from "./components/Global/LandingPage";

const App: React.FC = () => {
  const { isAuthenticated } = useAuthStore();
  return (
    <Router>
      <Routes>
        <Route
          path="/"
          element={isAuthenticated ? <Layout /> : <LandingPage />}
        >
          {isAuthenticated && (
            <>
              <Route index element={<TaskList />}></Route>
              <Route path="tasks/:id" element={<TaskDetail />}></Route>
              <Route path="about" element={<About />}></Route>
              <Route path="tasks/add" element={<AddTask />} />
              <Route path="tasks/edit/:id" element={<UpdateTask />} />
            </>
          )}
          <Route path="*" element={<Navigate to="/" />} />
        </Route>
      </Routes>
    </Router>
  );
};

export default App;
