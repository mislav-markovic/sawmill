import SystemIndex from "../components/System/SystemIndex.vue";
import Component from "../components/Component/Component.vue";
import SystemDetail from "../components/System/SystemDetail.vue";
import ParsingRulesForm from "../components/ParsingRules/ParsingRulesForm.vue";
import RawLogForm from "../components/RawLog/RawLogForm.vue";
import SettingsForm from "../components/Settings/SettingsForm.vue";
import SystemReport from "../components/Reports/SystemReport.vue"

export default [
  { path: "/", component: SystemIndex },
  { path: "/settings", component: SettingsForm },
  { path: "/reports/:reportId", component: SystemReport, props: (route) => { return { reportId: parseInt(route.params.reportId) } } },
  { path: "/rawlog/:componentId", component: RawLogForm, props: true },
  { path: "/system", component: SystemIndex },
  { path: "/system/:systemId", component: SystemDetail, props: (route) => { return { systemId: parseInt(route.params.systemId) } } },
  { path: "/component/:componentId", component: Component, props: (route) => { return { componentId: parseInt(route.params.componentId) } } },
  { path: "/parsingrules/:forComponent", component: ParsingRulesForm, props: (route) => { return { forComponent: parseInt(route.params.forComponent) } } },
  { path: "/parsingrules/:forComponent/:parsingRulesId", component: ParsingRulesForm, props: parsingRulesEdit },
];

function parsingRulesEdit(route) {
  return {
    forComponent: parseInt(route.params.forComponent),
    parsingRuleId: parseInt(route.params.parsingRulesId),
    isEdit: true,
  }
}